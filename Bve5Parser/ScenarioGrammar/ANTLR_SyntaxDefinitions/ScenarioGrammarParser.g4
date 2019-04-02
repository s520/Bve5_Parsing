parser grammar ScenarioGrammarParser;

options {
	tokenVocab=ScenarioGrammarLexer;
}

root :
	BVETS SCENARIO version=NUM encoding? statement* EOF
	;

statement :
	  stateName=ROUTE PATH_EQUAL (weight_path (SECTION weight_path)*)? (INPUT_PATH_END | PATH_END)?		#routeState
	| stateName=VEHICLE PATH_EQUAL (weight_path (SECTION weight_path)*)? (INPUT_PATH_END | PATH_END)?	#vehicleState
	| stateName=TITLE string																			#titleState
	| stateName=IMAGE string																			#imageState
	| stateName=ROUTETITLE string																		#routeTitleState
	| stateName=VEHICLETITLE string																		#vehicleTitleState
	| stateName=AUTHOR string																			#authorState
	| stateName=COMMENT string																			#commentState
	;

encoding returns [string text] :
	ENCODE v=encoding_text ENCODE_END? { $text = $v.text; }
	;

encoding_text :
	ENCODE_CHAR*
	;

string returns [string text] :
	EQUAL v=string_text INPUT_TEXT_END? { $text = $v.text; }
	;

string_text :
	INPUT_TEXT_CHAR*
	;

weight_path :
	path=FILE_PATH (ASTERISK weight=W_NUM)?
	;
