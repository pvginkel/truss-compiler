@echo off

mkdir tmp
..\..\Libraries\AntlrTools\Antlr3.exe -o tmp TrussPreProcessor.g
move tmp\TrussPreProcessorLexer.cs TrussPreProcessorLexer.Generated.cs > NUL
move tmp\TrussPreProcessorParser.cs TrussPreProcessorParser.Generated.cs > NUL
move tmp\* . > NUL
rmdir tmp

pause
