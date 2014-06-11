package truss.compiler.preprocessor;

import org.antlr.runtime.ANTLRStringStream;
import org.antlr.runtime.CommonTokenStream;
import org.antlr.runtime.RecognitionException;
import org.apache.commons.lang.Validate;

import java.util.HashSet;
import java.util.Set;
import java.util.Stack;

public class PreProcessor {
    private final char[] data;
    private final Set<String> defines;
    private final Stack<State> includes = new Stack<>();
    private boolean include;

    public static char[] process(char[] data, int size, Set<String> defines) throws PreProcessException {
        return new PreProcessor(data, size, defines).data;
    }

    private PreProcessor(char[] data, int size, Set<String> defines) throws PreProcessException {
        Validate.notNull(data, "data");

        this.defines = new HashSet<>(defines);
        this.data = parse(data, size);
    }

    private char[] parse(char[] data, int size) throws PreProcessException {
        StringBuilder out = new StringBuilder();
        StringBuilder directive = new StringBuilder();

        boolean hadNonSpace = false;
        boolean hadStart = false;
        boolean hadEnd = false;
        int line = 1;
        include = true;

        for (int i = 0; i <= size; i++) {
            boolean atEnd = i == size;
            char c = atEnd ? '\n' : data[i];

            // Always add the newline to out to preserve line count
            if (c == '\n' && !atEnd) {
                out.append(c);
                hadNonSpace = false;
            }

            if (hadStart) {
                if (c == '\n') {
                    parseDirective(directive.toString(), line);

                    hadStart = false;
                    hadEnd = false;

                    directive.setLength(0);
                } else if (!hadEnd) {
                    if (c == '/' && i < data.length - 1 && data[i + 1] == '/') {
                        hadEnd = true;
                    } else {
                        directive.append(c);
                    }
                }
            } else if (c != '\n') {
                if (c == '#' && !hadNonSpace) {
                    hadStart = true;
                } else if (include) {
                    out.append(c);
                    if (!Character.isWhitespace(c)) {
                        hadNonSpace = true;
                    }
                }
            }

            if (c == '\n' && !atEnd) {
                line++;
            }
        }

        if (!includes.empty()) {
            throw new PreProcessException("Missing #endif", line);
        }

        return out.toString().toCharArray();
    }

    private void parseDirective(String input, int line) throws PreProcessException {
        TrussPreProcessorLexer lexer = new TrussPreProcessorLexer(new ANTLRStringStream(input));
        TrussPreProcessorParser parser = new TrussPreProcessorParser(new CommonTokenStream(lexer));

        parser.setDefines(defines);

        try {
            addDirective(parser.directive(), line);
        } catch (RecognitionException e) {
            throw new PreProcessException("Cannot parse pre processor directive", line);
        }
    }

    private void addDirective(Directive directive, int line) throws PreProcessException {
        switch (directive.getKind()) {
            case IF:
                includes.push(((IfDirective)directive).isExpression() ? State.INCLUDE : State.EXCLUDE);

                recalculateIncludes();
                break;

            case ELIF:
                verifyHadIf("#elif", line);

                adjustIncludes(((ElIfDirective)directive).isExpression());

                recalculateIncludes();
                break;

            case ELSE:
                verifyHadIf("#else", line);

                adjustIncludes(true);

                recalculateIncludes();
                break;

            case ENDIF:
                verifyHadIf("#endif", line);

                includes.pop();

                recalculateIncludes();
                break;

            case DEFINE:
                if (include) {
                    DefineDirective define = (DefineDirective)directive;

                    if (define.isDefine()) {
                        defines.add(define.getIdentifier());
                    } else {
                        defines.remove(define.getIdentifier());
                    }
                }
                break;

            default:
                assert false;
        }
    }

    private void adjustIncludes(boolean expression) {
        if (includes.pop() == State.EXCLUDE) {
            includes.push(expression ? State.INCLUDE : State.EXCLUDE);
        } else {
            includes.push(State.HAD_INCLUDE);
        }
    }

    private void recalculateIncludes() {
        this.include = true;

        for (State include : includes) {
            if (!include.include) {
                this.include = false;
                return;
            }
        }
    }

    private void verifyHadIf(String directive, int line) throws PreProcessException {
        if (includes.empty()) {
            throw new PreProcessException(directive + " without a matching #if", line);
        }
    }

    private enum State {
        INCLUDE(true),
        EXCLUDE(false),
        HAD_INCLUDE(false);

        final boolean include;

        State(boolean include) {
            this.include = include;
        }
    }
}
