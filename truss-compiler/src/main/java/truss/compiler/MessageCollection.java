package truss.compiler;

import org.apache.commons.lang.Validate;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class MessageCollection {
    private final List<Message> messages = new ArrayList<>();
    private final List<Message> unmodifiableMessages = Collections.unmodifiableList(messages);
    private int errors;
    private int warnings;
    private int infos;

    public boolean hasErrors() {
        return errors > 0;
    }

    public int getErrors() {
        return errors;
    }

    public boolean hasErrorsOrWarnings() {
        return errors > 0 || warnings > 0;
    }

    public int getWarnings() {
        return warnings;
    }

    public boolean hasMessages() {
        return messages.size() > 0;
    }

    public int getInfos() {
        return infos;
    }

    public List<Message> getMessages() {
        return unmodifiableMessages;
    }

    public void add(Message message) {
        Validate.notNull(message, "message");

        messages.add(message);

        switch (message.getType().getSeverity()) {
            case ERROR: errors++; break;
            case WARN: warnings++; break;
            case INFO: infos++; break;
        }
    }

    public void add(MessageType type, Object... args) {
        add(new Message(type, args));
    }

    public void add(MessageType type, String fileName, Object... args) {
        add(new Message(type, fileName, args));
    }

    public void add(MessageType type, Span span, Object... args) {
        add(new Message(type, span, args));
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();

        for (Message message : messages) {
            sb.append(message.toString());
            sb.append('\n');
        }

        return sb.toString();
    }
}
