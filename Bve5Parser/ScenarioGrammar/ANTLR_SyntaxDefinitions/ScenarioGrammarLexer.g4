lexer grammar ScenarioGrammarLexer;

// File header
BVETS : B V E T S;
SCENARIO : S C E N A R I O;
ENCODE : ':' -> pushMode(ENCODING_MODE);

// Scenario descriptions
ROUTE : R O U T E -> pushMode(PATH_MODE);
VEHICLE : V E H I C L E -> pushMode(PATH_MODE);
TITLE : T I T L E;
IMAGE : I M A G E;
ROUTETITLE : R O U T E T I T L E;
VEHICLETITLE : V E H I C L E T I T L E;
AUTHOR : A U T H O R;
COMMENT : C O M M E N T;

EQUAL : '=' -> pushMode(INPUT_TEXT_MODE);
NUM : [0-9]+ ('.' [0-9]*)?
	  | '.' [0-9]+
;
WS : [\t \r\n]+ -> skip;
ESCAPE_COMMENT : SECTION_COMMENT -> skip;

fragment NEWLINE: '\r' '\n'? | '\n';
fragment SECTION_WS: [\t ]+;
fragment SECTION_COMMENT: ('#' | ';') ~[\r\n]*;

// Ignore case
fragment A : [aA];
fragment B : [bB];
fragment C : [cC];
fragment D : [dD];
fragment E : [eE];
fragment F : [fF];
fragment G : [gG];
fragment H : [hH];
fragment I : [iI];
fragment J : [jJ];
fragment K : [kK];
fragment L : [lL];
fragment M : [mM];
fragment N : [nN];
fragment O : [oO];
fragment P : [pP];
fragment Q : [qQ];
fragment R : [rR];
fragment S : [sS];
fragment T : [tT];
fragment U : [uU];
fragment V : [vV];
fragment W : [wW];
fragment X : [xX];
fragment Y : [yY];
fragment Z : [zZ];

mode ENCODING_MODE;
E_WS : SECTION_WS -> skip;
ENCODE_END : NEWLINE -> popMode;
ENCODE_CHAR : .;

mode PATH_MODE;
P_WS: SECTION_WS -> skip;
PATH_END: NEWLINE -> popMode;
PATH_EQUAL: '=' -> pushMode(INPUT_PATH_MODE);

mode INPUT_PATH_MODE;
IP_WS: SECTION_WS -> skip;
IP_COMMENT: SECTION_COMMENT -> skip;
INPUT_PATH_END: NEWLINE -> mode(DEFAULT_MODE);
ASTERISK : '*' -> pushMode(WEIGHTING_MODE);
SECTION : '|';
FILE_PATH : ~[*|#;\r\n]+;

mode WEIGHTING_MODE;
W_WS : SECTION_WS -> skip;
W_NUM : NUM -> popMode;

mode INPUT_TEXT_MODE;
IT_WS : SECTION_WS -> skip;
IT_COMMENT : SECTION_COMMENT -> skip;
INPUT_TEXT_END : NEWLINE -> popMode;
INPUT_TEXT_CHAR : .;
