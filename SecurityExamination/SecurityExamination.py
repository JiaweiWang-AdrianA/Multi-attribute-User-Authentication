import xlrd
import xlwt
import random

'''
	this policy is based on the rule:
		If there is only one difference between the attribute values of Obj1 and Obj2, which means Obj1:{...,Typei:Valueij,...} and Obj2:{...,Typei:Valueik,...} (Valueij not equal to Valueik), and Obj1 is correct but Obj2 is not. Then we can know the Valueij is the selected attribute values, while Valueik is not, and Typei is the selected attribute type.

	#Data Struct:

	String[] AttrType = [type1, type2, type3, ...]

	Dict AttrValue = {	type1:[value11, value12, value13, ...], 
						type2:[value21, value22, value23, ...], 
						type3:[value31, value32, value33, ...], 
						..., 
					 }

	Dict Object = {	AttrType[1]: AttrValue[AttrType[1]][x1], 
					AttrType[2]: AttrValue[AttrType[2]][x2], 
					AttrType[3]: AttrValue[AttrType[3]][x3], 
					..., 
					AttrType[TypeLen]:AttrValue[AttrType[TypeLen]][xTypeLen] 
				 }

	int[TypeLen] guessType ; 
		# guessType[i]:	== -1: Attarcker don't know if AttrType[i] was selected or not; 
						==  0: AttrType[i] was not selected;
						==  1: AttrType[i] was selected;

	Dict guessValue = {	type1:[isValueSelected11, isValueSelected12, isValueSelected13, ...], 
						type2:[isValueSelected21, isValueSelected22, isValueSelected23, ...], 
						type3:[isValueSelected31, isValueSelected32, isValueSelected33, ...], 
						..., 
					  }
		# guessValue[typei][k]:	== -1: Attarcker don't know if AttrValue[typei][k] was selected or not; 
								==  0: AttrValue[typei][k] was not selected;
								==  1: AttrValue[typei][k] was selected;
'''

# the number of Attribute Type
AttrTypeLen = 0

# the number of every Attribute Value
AttrValueLen = 0

# Attribute Type list
AttrType = []

# Attribute Value dictionary
AttrValue = {}

# password Value dictionary
passValue = {}

# guess Type list
guessType = []

# guess Value dictionary
guessValue = {}

# guess Password Value dictionary
guessPassValue = {}

# the number of objects during authentication
AuthObjNum = 0

# the objects in authentication
authObjs = []

# the correct objects (the objects match the passValue which need to be selected during authentiacation)
correctObjs = []

# the previously known correct objects(the attacker see the user selected these objects)
preKnownCoObjs = []

# the previously known wrong objects(the attacker see the user did't select these objects)
preKnownWrObjs = []

# Minimum correct object number(at least select miniCoObjNum objects) during one authentication
miniCoObjNum = 0


# genarate the objects of authentication
def genAuthObjs():
	#genarate AuthObjNum objects
	authObjsTemp = []
	attemptCount = 0
	objCount = 0
	while objCount < AuthObjNum:
		obj = []
		# At least miniCoObjNum(int) objects are correct
		if objCount < miniCoObjNum:
			for i in range(0,len(AttrType)):
				attribute = "Attr" + chr(i+ord('A'))
				AttrLen = 0
				# if this attribute type is selected
				if True in passValue[attribute]:
					for vi in passValue[attribute]:
						if vi: 
							AttrLen += 1
				else:
					AttrLen = len(AttrValue[AttrType[i]])
				attrVt = random.randint(0,AttrLen-1)
				obj.append(attrVt)
		else:
			# genarate random attribute values of every object
			for i in range(0,len(AttrType)):
				AttrLen = len(AttrValue[AttrType[i]])
				attrVt = random.randint(0,AttrLen-1)
				obj.append(attrVt)
		# the genarated object has at least two different attributes comparing to every object genarated before
		# if this condition cannot be met after 20 consecutive attempts, authObjFalg is always True
		authObjFalg = True
		for k in range(0,objCount):
			# after 20 consecutive attempts, don't need to compute the objDiffCount (authObjFalg is always True)
			if attemptCount >= 20:
				break
			objDiffCount = 0
			for i in range(0, len(AttrType)):
				if authObjsTemp[k][i] != obj[i]:
					objDiffCount += 1
			if objDiffCount <= 1:
				authObjFalg = False
				attemptCount += 1
				break
		if authObjFalg:
			authObjsTemp.append(obj)
			# if the attemp times <20, then count the number of attempts from the beginning(0)
			if attemptCount < 20:
				attemptCount = 0
			objCount += 1

	return authObjsTemp


#genarate the passwords under different passSettings
def genPassword(passSetting):
	global passValue
	#initialize passValue = {}
	for i in range(0,AttrTypeLen):
		attribute = "Attr" + chr(i+ord('A'))
		passValue[attribute] = [False] * AttrValueLen
	password={}
	
	#passValueLists
	if "+" in passSetting:
		passValueNums = passSetting.split("+")
		passTypeNum = len(passValueNums)
		for i in range(0,passTypeNum):
			attribute = "Attr" + chr(i+ord('A'))
			for j in range(0,int(passValueNums[i])):
				passValue[attribute][j] = True
	
	#passTypeLen * passValueLen
	elif "*" in passSetting:
		passTypeMutiValue = passSetting.split("*")
		if len(passTypeMutiValue) != 2:
			return "error"
		else:
			passTypeNum = int(passTypeMutiValue[0])
			passValueNum = int(passTypeMutiValue[1])
		if passTypeNum > AttrTypeLen or passValueNum > AttrValueLen:
			return "error"
		for i in range(0,passTypeNum):
			attribute = "Attr" + chr(i+ord('A'))
			for j in range(0,passValueNum):
				passValue[attribute][j] = True
		#print(passValue)
	else:
		return "error"

	return password


#get the objects need to be selected during authentication
def getCorrectObjs(passValue,authObjs):
	#the list of selected attributes
	correctObjs = []
	isSelectedAttrs = []
	for v in passValue:
		for b in passValue[v]:
			if b:
				isSelectedAttrs.append(v)
				break
	#compare whether the authObj matches the passValue or not	
	for k in range(0,len(authObjs)):
		isCorrectObj = True
		#judge every attribute value
		for i in range(0,len(AttrType)):
			attrT = AttrType[i]
			if attrT in isSelectedAttrs and not passValue[attrT][authObjs[k][i]]:
				isCorrectObj = False
				break
		if isCorrectObj:
			correctObjs.append(authObjs[k])

	return correctObjs


# getAllCorrectObjs() returns all the correct objects
def getAllCorrectObjs(correctObjs):
	allCorrectObjs = []
	for obj in correctObjs:
		objt = tuple(obj)
		if objt not in allCorrectObjs:
			allCorrectObjs.append(objt)

	return allCorrectObjs

# getAllWrongObjs() returns all the wrong objects
def getAllWrongObjs():
	allAuthObjs=[]
	allCorrectObjs=[]
	for obj in authObjs:
		objt = tuple(obj)
		allAuthObjs.append(objt)
	allAuthObjs = set(allAuthObjs)
	allCorrectObjs = set(getAllCorrectObjs(correctObjs))
	wrongObjs = allAuthObjs.difference(allCorrectObjs)

	return list(wrongObjs)


#objsDifference(obj1,obj2) returns the difference list (Obj1-obj2)
def objsDifference(obj1,obj2):
	objDiff=[]
	for i in range(0,len(obj1)):
		if obj1[i] != obj2[i]:
			attrT = AttrType[i]
			objDiff.append(AttrValue[attrT][obj1[i]])

	return objDiff


#isValueSelected(value) judge whether the value in guessValue is selected or not
def isValueSelected(value):
	for k in AttrValue:
		for vi in range(0,len(AttrValue[k])):
			if AttrValue[k][vi] == value:
				if guessValue[k][vi] == 1:
					return True
				else:
					return False


#isValueSelected(value) judge whether the value in guessValue isn't selected or not
def isValueNotSelected(value):
	for k in AttrValue:
		for vi in range(0,len(AttrValue[k])):
			if AttrValue[k][vi] == value:
				if guessValue[k][vi] == 0:
					return True
				else:
					return False


def isValueSelectedUnkown(value):
	for k in AttrValue:
		for vi in range(0,len(AttrValue[k])):
			if AttrValue[k][vi] == value:
				if guessValue[k][vi] == -1:
					return True
				else:
					return False


#isValueTypeSelected(value) judge whether the type of value in guessType is selected or not
def isValueTypeSelected(value):
	ki=0
	for k in AttrValue:
		for vi in range(0,len(AttrValue[k])):
			if AttrValue[k][vi] == value:
				if guessType[ki] == 1:
					return True
				else:
					return False
		ki+=1


#isValueTypeNotSelected(value) judge whether the type of value in guessType isn't selected or not
def isValueTypeNotSelected(value):
	ki=0
	for k in AttrValue:
		for vi in range(0,len(AttrValue[k])):
			if AttrValue[k][vi] == value:
				if guessType[ki] == 0:
					return True
				else:
					return False
		ki+=1


#getGuessTypeValue(attrType) returns the value(-1/0/1) of attrType in guessType
def getGuessTypeValue(attrType):
	for i in range(0,len(AttrType)):
		if AttrType[i] == attrType:
			return guessType[i]


def getCorrectValueFromDiff(objC,objW):
	#objsDifference(obj1,obj2) returns the difference list (Obj1-obj2)
	diffCorrectValues = objsDifference(objC,objW)
	if len(diffCorrectValues) == 1:
		#print("[len=1]"+diffCorrectValues[0])
		return diffCorrectValues[0]
	for v in diffCorrectValues:
		if (isValueSelected(v) and isValueTypeSelected(v)) or isValueTypeNotSelected(v):
			#print("[remove] "+v)
			diffCorrectValues.remove(v)
	if len(diffCorrectValues) == 1:
		#print(objC)
		#print(objW)
		#print("diffCorV :",end=" ")
		#print(diffCorrectValues[0])
		return diffCorrectValues[0]


def getWrongValueFromDiff(objC,objW):
	diffWrongValues = objsDifference(objW,objC)
	if len(diffWrongValues) == 1:
		return diffWrongValues[0]
	for v in diffWrongValues:
		if (isValueSelected(v) and isValueTypeSelected(v)) or isValueTypeNotSelected(v):
			diffWrongValues.remove(v)
	if len(diffWrongValues) == 1:
		return diffWrongValues[0]


def getTypeFromDiff(obj1,obj2):
	diffTypes = []
	for i in range(0,len(obj1)):
		if obj1[i] != obj2[i]:
			diffTypes.append(AttrType[i])
	if len(diffTypes) == 1:
		return diffTypes[0]


#set attrType in guessType = x (x = -1/0/1)
def setGuessType(attrType,x):
	for i in range(0,len(AttrType)):
		if attrType == AttrType[i]:
			guessType[i] = x
			return


#set attrValue in guessValue = x (x = -1/0/1)
def setGuessValue(attrValue,x):
	for k in AttrValue:
		for vi in range(0,len(AttrValue[k])):
			if AttrValue[k][vi] == attrValue:
				guessValue[k][vi] = x
				return


# if all values in an attribute type is 1, then this type is not selected
# if one value in an attribute type is 0, then this type is selected
def updateType():
	updateFlag  = False
	# if all values in an attribute type is 1, then this type is not selected
	for i in range(0,len(AttrType)):
		flag = True
		for v in guessValue[AttrType[i]]:
			if v != 1:
				flag = False
				break
		if flag and guessType[i] == -1:
			setGuessType(AttrType[i],0)
			updateFlag = True
	# if one value in an attribute type is 0, then this type is selected
	for i in range(0,len(AttrType)):
		flag = False
		for v in guessValue[AttrType[i]]:
			if v == 0:
				flag = True
				break
		if flag and guessType[i] == -1:
			setGuessType(AttrType[i],1)
			updateFlag = True
	return updateFlag


def AttackerPolicy():
	# getAllCorrectObjs() returns all the correct objects
	global correctObjs, preKnownCoObjs, preKnownWrObjs 
	correctObjs = getCorrectObjs(passValue,authObjs)
	allCorrectObjs = getAllCorrectObjs(correctObjs)
	allWrongObjs = getAllWrongObjs()
	preKnownCoObjs = correctObjs
	preKnownWrObjs = allWrongObjs

	update = True
	# if there are not wrong objects, then set all guessValue = 1
	if len(allWrongObjs) == 0:
		for AType in AttrType:
			setGuessType(AType,1)
			for AValue in AttrValue[AType]:
				setGuessValue(AValue,1)
		update =False
	# find all the <Obj1,Obj2> followed the rule  
	while update:
		update = False
		for objC in allCorrectObjs:
			for objW in allWrongObjs:
				#objC,objW = list(objC),list(objW)
				diffCorrValue = getCorrectValueFromDiff(objC,objW)
				diffWronValue = getWrongValueFromDiff(objC,objW)
				diffType = getTypeFromDiff(objC,objW)
				if getGuessTypeValue(diffType) == -1 or isValueSelectedUnkown(diffCorrValue) or isValueSelectedUnkown(diffWronValue):
					update = True
				setGuessType(diffType,1)
				setGuessValue(diffCorrValue,1)
				setGuessValue(diffWronValue,0)
				if updateType():
					update = True


#fill the guessPassValue base on guessValue
def getGuessPassValue():
	global guessPassValue
	for i in range(0,len(AttrType)):
		attrT = AttrType[i]
		if guessType[i] == 0:
			guessPassValue[attrT] = [False]*len(AttrValue[attrT])
		else:
			guessL = []
			for k in guessValue[attrT]:
				if k == -1:
					kv = random.randint(0,1)
				else:
					kv = k
				if kv == 1:
					guessL.append(True)
				else:
					guessL.append(False)
			guessPassValue[attrT] = guessL


def securityTest(passSetting):
	global authObjs, correctObjs, guessType, guessValue, guessPassValue
	guessType = []
	# reset the guess password
	for i in range(0,AttrTypeLen):
		attributeOrd = chr(i+ord('A'))
		attribute = "Attr" + attributeOrd
		guessType.append(-1)
		guessValue[attribute] = [-1] * AttrValueLen
		guessPassValue[attribute] = [False] * AttrValueLen
	# setting the password
	genPassword(passSetting)

	# generate three authentication seen by the attacker
	authObjs = [] ; correctObjs = []
	for i in range (0,3):
		authObjs.extend(genAuthObjs())

	# attacker execute AttackerPolicy to genarate the guessValue
	AttackerPolicy()

	# The attacker tried three times to authenticate
	authObjs = [] ; correctObjs = []
	authObjs = genAuthObjs()
	correctObjs = set(getAllCorrectObjs(getCorrectObjs(passValue,authObjs)))
	for i in range(0,3):	
		getGuessPassValue()
		#guessCoObjs = set(getAllCorrectObjs(getCorrectObjs(guessPassValue,authObjs)))
		guessCoObjs = getCorrectObjs(guessPassValue,authObjs)
		# remove the known wrong objects
		for obj in guessCoObjs:
			if obj in preKnownWrObjs:
				guessCoObjs.remove(obj)
		# add the known correct objects 
		for obj in authObjs:
			if obj in preKnownCoObjs:
				guessCoObjs.append(obj)
		guessCoObjs = getAllCorrectObjs(guessCoObjs)
		# if not guessCoObjs, then choose miniCoObjNum object at random (except the preKnownWrObjs)
		lenGuessCoObjs = 0
		# the authObjs may contain several same objects
		for getAllCorrectObj in guessCoObjs:
			lenGuessCoObjs += authObjs.count(list(getAllCorrectObj))
		while lenGuessCoObjs < miniCoObjNum:
			guessCoObjOrd = random.randint(0,AuthObjNum-1)
			guessCoObj = authObjs[guessCoObjOrd]
			if guessCoObj not in preKnownWrObjs and tuple(guessCoObj) not in guessCoObjs:
				guessCoObjs.append(tuple(guessCoObj))
				lenGuessCoObjs += authObjs.count(guessCoObj)

		guessCoObjs = set(guessCoObjs)
		if correctObjs == guessCoObjs:
			#print("!!",end="")
			return True

	return False


#initialize the AttrType, AttrValue, passValue, guessType, guessValue, guessPassValue
def init():
	global AttrType, AttrValue, passValue, guessType, guessValue, guessPassValue
	AttrType = []
	guessType = []
	for i in range(0,AttrTypeLen):
		attributeOrd = chr(i+ord('A'))
		attribute = "Attr" + attributeOrd
		AttrType.append(attribute)
		guessType.append(-1)		
		passValue[attribute] = [False] * AttrValueLen
		guessValue[attribute] = [-1] * AttrValueLen
		guessPassValue[attribute] = [False] * AttrValueLen
		#initialize AttrValue
		AttrValue[attribute] = []
		for j in range(0,AttrValueLen):
			attributeValue = attributeOrd + str(j)
			AttrValue[attribute].append(attributeValue)


def test(attrTypeLens,attrValueLens,authObjNums,passSettings,attackNum):
	global AttrTypeLen, AttrValueLen, AuthObjNum, miniCoObjNum

	workbook = xlwt.Workbook()

	print("[Test Times] " + str(attackNum))

	for AttrTypeLen in attrTypeLens:
		print("===========================================================")
		for AttrValueLen in attrValueLens:
			print("[AttrTypeNum * AttrValueNum] " + str(AttrTypeLen) + " * " + str(AttrValueLen))
			print("\t", end = "")

			# built new sheet
			sheet_name = str(AttrTypeLen) + "x" + str(AttrValueLen)
			sheet = workbook.add_sheet(sheet_name)
			cCount = 1
			# the first row of the sheet
			for passSetting in passSettings:
				print(passSetting, end = "\t")
				sheet.write(0, cCount, passSetting)
				cCount += 1
			print()

			# the rest rows of the sheet
			rCount = 1
			for AuthObjNum in authObjNums:
				print("-----------------------------------------------------------")
				for miniCoObjNum in range(1,AuthObjNum):
					print(str(miniCoObjNum)+ " / " + str(AuthObjNum), end ="\t")
					# the first cell of the row
					sheet.write(rCount, 0, str(miniCoObjNum)+ " / " + str(AuthObjNum))

					init()
					cCount = 0
					for passSetting in passSettings:
						cCount += 1
						# if passSetting is not applicable
						if genPassword(passSetting) is "error":
							continue
						attackSeccessNum = 0
						for i in range(0,attackNum):
							if securityTest(passSetting):
								attackSeccessNum += 1
						seccessRate = round(attackSeccessNum/attackNum * 100 , 1)
						#print("[Security passSetting] " + passSetting, end = "\t")
						#print("[Seccess Rate] " + str(seccessRate))

						print(str(seccessRate), end = "\t")
						# write the seccessRate to the sheet
						sheet.write(rCount, cCount, seccessRate)
						workbook.save('./testData.xls')
					print()
					rCount += 1


if __name__ == '__main__':
	attrTypeLens = [4,5,6]
	attrValueLens = [5,10,15,30,60]
	authObjNums = [3,6,9,12,15]
	passSettings = ["1*2","1*3","1*4","2*1","2*2","2*3","2*4","3*1","3*2","3*3","3*4","4*1","4*2","4*3","4*4"]
	attackNum = 100
	test(attrTypeLens,attrValueLens,authObjNums,passSettings,attackNum)
	print("===========================================================")
	print("The man-in-the-room attack simulation is over, the complete result is in file [testData.xls]")
	



#print(guessType)
#print(guessPassValue)
#print(passValue)
