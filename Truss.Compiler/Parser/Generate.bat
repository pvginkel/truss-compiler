@echo off

mkdir tmp
..\..\Libraries\AntlrTools\Antlr3.exe -o tmp Truss.g
move tmp\TrussLexer.cs TrussLexer.Generated.cs > NUL
move tmp\TrussParser.cs TrussParser.Generated.cs > NUL
move tmp\* . > NUL
rmdir tmp

pause
