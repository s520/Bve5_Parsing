parser grammar MapV2GrammarParser;

options {
	tokenVocab=MapV2GrammarLexer;
}

root :
	BVETS MAP version=NUM encoding? (statement STATE_END+)* EOF
	;

statement :
	  distance							#distState
	| INCLUDE include					#includeState
	| CURVE curve 						#curveState
	| GRADIENT gradient 				#gradientState
	| TRACK track 						#trackState
	| STRUCTURE structure 				#structureState
	| REPEATER repeater 				#repeaterState
	| BACKGROUND background 			#backgroundState
	| STATION station 					#stationState
	| SECTION section 					#sectionState
	| SIGNAL signal 					#signalState
	| BEACON beacon 					#beaconState
	| SPEEDLIMIT speedLimit 			#speedLimitState
	| PRETRAIN preTrain 				#preTrainState
	| LIGHT light 						#lightState
	| FOG fog 							#fogState
	| DRAWDISTANCE drawDistance 		#drawDistanceState
	| CABILLUMINANCE cabIlluminance 	#cabIlluminanceState
	| IRREGULARITY irregularity 		#irregularityState
	| ADHESION adhesion 				#adhesionState
	| SOUND sound 						#soundState
	| SOUND3D sound3d 					#sound3dState
	| ROLLINGNOISE rollingNoise 		#rollingNoiseState
	| FLANGENOISE flangeNoise 			#flangeNoiseState
	| JOINTNOISE jointNoise 			#jointNoiseState
	| TRAIN train 						#trainState
	| LEGACY legacy						#legacyState
	| varAssign 						#varAssignState
	;

// Current discance
distance :
	expr
	;

// Insert other map file
include :
	path=expr
	;

// Lateral curve of own track
curve :
	  DOT func=(SET_GAUGE | GAUGE) OPN_PAR value=nullableExpr CLS_PAR
	| DOT func=SET_CENTER OPN_PAR x=nullableExpr CLS_PAR
	| DOT func=SET_FUNCTION OPN_PAR id=nullableExpr CLS_PAR
	| DOT func=BEGIN_TRANSITION OPN_PAR CLS_PAR
	| DOT func=(BEGIN | BEGIN_CIRCULAR) OPN_PAR radius=nullableExpr (COMMA cant=nullableExpr)? CLS_PAR
	| DOT func=END OPN_PAR CLS_PAR
	| DOT func=INTERPOLATE OPN_PAR CLS_PAR
	| DOT func=INTERPOLATE OPN_PAR radiusE=expr CLS_PAR
	| DOT func=INTERPOLATE OPN_PAR radius=nullableExpr COMMA cant=nullableExpr CLS_PAR
	| DOT func=CHANGE OPN_PAR radius=nullableExpr CLS_PAR
	;

// Gradient of own track
gradient :
	  DOT func=BEGIN_TRANSITION OPN_PAR CLS_PAR
	| DOT func=(BEGIN | BEGIN_CONST) OPN_PAR gradientArgs=nullableExpr CLS_PAR	// Since the argument name gradient is the same, we use gradientArgs
	| DOT func=END OPN_PAR CLS_PAR
	| DOT func=INTERPOLATE OPN_PAR gradientArgsE=expr CLS_PAR
	;

// Other track
track :
	  OPN_BRA key=expr CLS_BRA DOT element=X_ELEMENT DOT func=INTERPOLATE OPN_PAR CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=X_ELEMENT DOT func=INTERPOLATE OPN_PAR xE=expr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=X_ELEMENT DOT func=INTERPOLATE OPN_PAR x=nullableExpr COMMA radius=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=Y_ELEMENT DOT func=INTERPOLATE OPN_PAR CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=Y_ELEMENT DOT func=INTERPOLATE OPN_PAR xE=expr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=Y_ELEMENT DOT func=INTERPOLATE OPN_PAR x=nullableExpr COMMA radius=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=POSITION OPN_PAR x=nullableExpr COMMA y=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=POSITION OPN_PAR x=nullableExpr COMMA y=nullableExpr COMMA radiusH=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=POSITION OPN_PAR x=nullableExpr COMMA y=nullableExpr COMMA radiusH=nullableExpr COMMA radiusV=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=SET_CENTER OPN_PAR x=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=SET_GAUGE OPN_PAR gauge=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=SET_FUNCTION OPN_PAR id=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=BEGIN_TRANSITION OPN_PAR CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=BEGIN OPN_PAR cant=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=END OPN_PAR CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT element=CANT_ELEMENT DOT func=INTERPOLATE OPN_PAR cant=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=CANT_ELEMENT OPN_PAR cant=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=GAUGE OPN_PAR gauge=nullableExpr CLS_PAR
	;

// Structure
structure :
	  DOT func=LOAD OPN_PAR path=expr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUT OPN_PAR trackKey=nullableExpr COMMA x=nullableExpr COMMA y=nullableExpr COMMA z=nullableExpr COMMA rx=nullableExpr COMMA ry=nullableExpr COMMA rz=nullableExpr COMMA tilt=nullableExpr COMMA span=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUT0 OPN_PAR trackKey=nullableExpr COMMA tilt=nullableExpr COMMA span=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUTBETWEEN OPN_PAR trackKey1=nullableExpr COMMA trackKey2=nullableExpr (COMMA flag=nullableExpr)? CLS_PAR
	;

// Continious structure
repeater :
	  OPN_BRA key=expr CLS_BRA DOT func=BEGIN OPN_PAR trackKey=nullableExpr COMMA x=nullableExpr COMMA y=nullableExpr COMMA z=nullableExpr COMMA rx=nullableExpr COMMA ry=nullableExpr COMMA rz=nullableExpr COMMA tilt=nullableExpr COMMA span=nullableExpr COMMA interval=nullableExpr exprArgs+ CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=BEGIN0 OPN_PAR trackKey=nullableExpr COMMA tilt=nullableExpr COMMA span=nullableExpr COMMA interval=nullableExpr exprArgs+ CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=END OPN_PAR CLS_PAR
	;

// Background
background :
	DOT func=CHANGE OPN_PAR structureKey=nullableExpr CLS_PAR
	;

// Station
station :
	  DOT func=LOAD OPN_PAR path=expr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUT OPN_PAR door=nullableExpr COMMA margin1=nullableExpr COMMA margin2=nullableExpr CLS_PAR
	;

// Section (block)
section :
	  DOT func=(BEGIN | BEGIN_NEW) OPN_PAR nullableExpr exprArgs* CLS_PAR
	| DOT func=SET_SPEEDLIMIT OPN_PAR nullableExpr exprArgs* CLS_PAR
	;

// Signal
signal :
	  DOT func=LOAD OPN_PAR path=expr CLS_PAR
	| DOT func=SPEEDLIMIT OPN_PAR nullableExpr exprArgs* CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUT OPN_PAR sectionArgs=nullableExpr COMMA trackKey=nullableExpr COMMA x=nullableExpr COMMA y=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUT OPN_PAR sectionArgs=nullableExpr COMMA trackKey=nullableExpr COMMA x=nullableExpr COMMA y=nullableExpr COMMA z=nullableExpr COMMA rx=nullableExpr COMMA ry=nullableExpr COMMA rz=nullableExpr COMMA tilt=nullableExpr COMMA span=nullableExpr CLS_PAR
	;

// Beacon (ground coil)
beacon :
	DOT func=PUT OPN_PAR type=nullableExpr COMMA sectionArgs=nullableExpr COMMA sendData=nullableExpr CLS_PAR
	;

// Speed limit
speedLimit :
	  DOT func=BEGIN OPN_PAR v=nullableExpr CLS_PAR
	| DOT func=END OPN_PAR CLS_PAR
	;

// Leading train
preTrain :
	DOT func=PASS OPN_PAR nullableExpr CLS_PAR
	;

// Light
light :
	  DOT func=AMBIENT OPN_PAR red=nullableExpr COMMA green=nullableExpr COMMA blue=nullableExpr CLS_PAR
	| DOT func=DIFFUSE OPN_PAR red=nullableExpr COMMA green=nullableExpr COMMA blue=nullableExpr CLS_PAR
	| DOT func=DIRECTION OPN_PAR pitch=nullableExpr COMMA yaw=nullableExpr CLS_PAR
	;

// Fog effect
fog :
	  DOT func=INTERPOLATE OPN_PAR CLS_PAR
	| DOT func=INTERPOLATE OPN_PAR densityE=expr CLS_PAR
	| DOT func=(INTERPOLATE | SET) OPN_PAR density=nullableExpr COMMA red=nullableExpr COMMA green=nullableExpr COMMA blue=nullableExpr CLS_PAR
	;

// Scenary draw distance
drawDistance :
	DOT func=CHANGE OPN_PAR value=nullableExpr CLS_PAR
	;

// Illuminance of instrument panel
cabIlluminance :
	  DOT func=INTERPOLATE OPN_PAR CLS_PAR
	| DOT func=(INTERPOLATE | SET) OPN_PAR valueE=expr CLS_PAR
	;

// Track irregularity
irregularity :
	DOT func=CHANGE OPN_PAR x=nullableExpr COMMA y=nullableExpr COMMA r=nullableExpr COMMA lx=nullableExpr COMMA ly=nullableExpr COMMA lr=nullableExpr CLS_PAR
	;

// Wheel-rail adhesion
adhesion :
	  DOT func=CHANGE OPN_PAR a=nullableExpr CLS_PAR
	| DOT func=CHANGE OPN_PAR a=nullableExpr COMMA b=nullableExpr COMMA c=nullableExpr CLS_PAR
	;

// Sound
sound :
	  DOT func=LOAD OPN_PAR path=expr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PLAY OPN_PAR CLS_PAR
	;

// Sound on ground
sound3d :
	  DOT func=LOAD OPN_PAR path=expr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=PUT OPN_PAR x=nullableExpr COMMA y=nullableExpr CLS_PAR
	;

// Wheel rolling sound
rollingNoise :
	DOT func=CHANGE OPN_PAR index=nullableExpr CLS_PAR
	;

// Flange rasping sound
flangeNoise :
	DOT func=CHANGE OPN_PAR index=nullableExpr CLS_PAR
	;

// Rail joint sound
jointNoise :
	DOT func=PLAY OPN_PAR index=nullableExpr CLS_PAR
	;

// Other train
train :
	  DOT func=ADD OPN_PAR trainKey=nullableExpr COMMA path=expr COMMA trackKey=nullableExpr COMMA direction=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=LOAD OPN_PAR path=expr COMMA trackKey=nullableExpr COMMA direction=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=ENABLE OPN_PAR time=nullableExpr CLS_PAR
	| OPN_BRA key=expr CLS_BRA DOT func=STOP OPN_PAR decelerate=nullableExpr COMMA stopTime=nullableExpr COMMA accelerate=nullableExpr COMMA speed=nullableExpr CLS_PAR
	;

// Legacy syntax
legacy :
	  DOT func=FOG OPN_PAR startArg=nullableExpr COMMA endArg=nullableExpr COMMA red=nullableExpr COMMA green=nullableExpr COMMA blue=nullableExpr CLS_PAR
	| DOT func=CURVE OPN_PAR radius=nullableExpr COMMA cant=nullableExpr CLS_PAR
	| DOT func=PITCH OPN_PAR rate=nullableExpr CLS_PAR
	| DOT func=TURN OPN_PAR slope=nullableExpr CLS_PAR
	;

// Variable declaration
varAssign :
	v=var EQUAL expr
	;

// Continuous mathematical argument
exprArgs :
	COMMA arg=nullableExpr
	;

nullableExpr :
	  expr
	| nullSyntax=NULL
	| /* epsilon */
	;

expr :
	  OPN_PAR expr CLS_PAR								#parensExpr
	| op=(PLUS | MINUS) expr							#unaryExpr
	| left=expr op=(MULT | DIV) right=expr				#infixExpr
	| left=expr op=(PLUS | MINUS | MOD) right=expr		#infixExpr
	| func=ABS OPN_PAR value=expr CLS_PAR				#absExpr
	| func=ATAN2 OPN_PAR y=expr COMMA x=expr CLS_PAR	#atan2Expr
	| func=CEIL OPN_PAR value=expr CLS_PAR				#ceilExpr
	| func=COS OPN_PAR value=expr CLS_PAR				#cosExpr
	| func=EXP OPN_PAR value=expr CLS_PAR				#expExpr
	| func=FLOOR OPN_PAR value=expr CLS_PAR				#floorExpr
	| func=LOG OPN_PAR value=expr CLS_PAR				#logExpr
	| func=POW OPN_PAR x=expr COMMA y=expr CLS_PAR		#powExpr
	| func=RAND OPN_PAR value=expr? CLS_PAR				#randExpr
	| func=SIN OPN_PAR value=expr CLS_PAR				#sinExpr
	| func=SQRT OPN_PAR value=expr CLS_PAR				#sqrtExpr
	| v=var												#varExpr
	| num=NUM											#numberExpr
	| str=string										#stringExpr
	| dist=DISTANCE										#distanceExpr
	;

encoding returns [string text] :
	ENCODE v=encoding_text ENCODE_END? { $text = $v.text; }
	;

encoding_text :
	ENCODE_CHAR*
	;

var returns [string varName]:
	VAR_START v=VAR { $varName = $v.text ;}
	;

string returns [string text]:
	QUOTE v=string_text RQUOTE { $text = $v.text ;}
	;

string_text :
	STRING_CHAR*
	;
