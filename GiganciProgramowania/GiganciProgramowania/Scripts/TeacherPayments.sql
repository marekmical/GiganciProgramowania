USE GiganciBaza
GO

DECLARE @LessonFinishedState NVARCHAR(10) = N'finished';
DECLARE @PaymentCalculationsStartDate DATE = (SELECT GETDATE() - 90);
DECLARE @DiagnosticLessonType NVARCHAR(50) = N'DemonstrationLesson1On1Online';
DECLARE @ActiveContractKind NVARCHAR(10) = N'Current';
DECLARE @DefaultCurrencyHourStake INT = 50;

SELECT		[LessonInfo].TeacherID, 
			CONCAT([User].Name, ' ', [User].Surname) as TeacherName, 
			CONCAT(SUM(IIF([Contract].Kind IS NOT NULL AND [Contract].Kind = @ActiveContractKind, [Contract].Rate, @DefaultCurrencyHourStake) * [LessonDatePerTimetable].HoursNumber),[CountryCurrency].CurrencySymbol) as Payment
FROM		[LessonInfo]
			LEFT JOIN [Timetable] ON [LessonInfo].TimetableID = [Timetable].TimetableID
			LEFT JOIN [Course] ON [Timetable].CourseID = [Course].CourseID
			LEFT JOIN [LessonDatePerTimetable] ON [LessonInfo].LessonDatePerTimetableID = [LessonDatePerTimetable].LessonDatePerTimetableId
			--Get Teacher Data
			LEFT JOIN [Teacher] ON [Teacher].TeacherID = [LessonInfo].TeacherID
			LEFT JOIN [User] ON [User].ID = [Teacher].UserID
			--Get Contract
			LEFT JOIN [Contract] ON [Contract].TeacherId = [LessonInfo].TeacherID
			--Get Currency Symbol
			LEFT JOIN [Localisation] ON [Timetable].LocalisationID = [Localisation].LocalisationID
			LEFT JOIN [City] ON [Localisation].CityId = [City].CityId
			LEFT JOIN [Voivodeship] ON [City].VoivodeshipId = [Voivodeship].VoivodeshipId
			LEFT JOIN [Country] ON [Voivodeship].CountryId = [Country].CountryId
			LEFT JOIN [CountryCurrency] ON [Country].CountryId = [CountryCurrency].CountryId
WHERE		LessonState = @LessonFinishedState AND
			LessonFinishedOn >= @PaymentCalculationsStartDate AND
			Course.CourseType = @DiagnosticLessonType
GROUP BY	[LessonInfo].TeacherID, [User].Name, [User].Surname, [CountryCurrency].CurrencySymbol;


GO