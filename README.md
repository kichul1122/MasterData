## 배치파일
* _All.bat : Genrator -> Convertor -> Copy 까지 한방에 진행한다(엑셀 형식 변경 되었을 때 추천)
* _ConvertorCopy.bat : Convertor -> Copy 를 진행 (엑셀 데이터만 변경 되었을때 추천)
* Validator : 데이터 검증

* GeneratorDirectory.bat : cs폴더에 MasterData.cs 및 Const.cs 생성
* GeneratorTable.bat : cs폴더에 있는 cs파일들 기반으로 table폴더에 Table.cs 생성
* ConvertorDirectory.bat : json폴더에 json생성

* CopyFilesCS.bat : cs폴더의 cs파일들을 클라이언트 Assets\_Scripts\MasterData 폴더로 복사
* CopyFilesTable.bat : table폴더의 table파일들을 클라이언트 Assets\_Scripts\MasterData\Table 폴더로 복사
* CopyFilesJson.bat : json폴더의 파일들을 클라이언트 Assets\Resources\MasterData 폴더로 복사

## exe
* MasterData.Generator.exe : 엑셀 형식으로 cs 파일생성 
* MasterMemory.Generator.exe : cs파일로 table 파일 생성
* MasterData.Convertor.exe : 엑셀 데이터로 json생성

## Generator가 실행 되어야하는 경우
엑셀의 형식이 변경된 경우
 - sheet.m : Column이 추가된 경우
 - sheet.c : Row가 추가된 경우	
 - sheet.e : Ennum이 추가된 경우	
 
## 테이블 형식	
* .m : 테이블 데이터
* .c : 상수형 데이터
* .e : Enum 정의
* .mn : 테이블 데이터 New (같은 데이터지만 파일 분리를 위해 사용하고 .mb와 쌍을 이루고, 최신 데이터 입력용)
* .mb : 테이블 데이터 Backup (같은 데이터지만 파일 분리를 위해 사용하고 .mb와 쌍을 이루고, 백업 데이터 입력용) 

### 자료형(Data Type)
- int, long, float, double, string
- List<X>

List<X> 셀 입력시 구분자는 (,) 입니다.

### 커스텀 클래스
추가 요청은 클라이언트 개발자에게 (MasterData.Core 프로젝트 - ConvertorCore.cs 참고)
- Customclass
- List<Customclass>

커스텀 클래스 셀 입력 형태 (X:X:X) 입니다.

	
### Attribute 및 API 는 https://github.com/Cysharp/MasterMemory 참고
* sample.xlsx 참고
* [PrimaryKey] : 중복 아이디 불허용
* [PrimaryKey, NonUnique] : 중복 아이디 허용
* [IgnoreMember] : 테이블 데이터에서 해당 컬럼을 무시한다(주석 또는 메모 용도로 사용), partial class에 선언 해서 프로그래머용 멤버변수로 사용 가능

* .m 파일은 [PrimaryKey]가 최소 1개 이상 필요
