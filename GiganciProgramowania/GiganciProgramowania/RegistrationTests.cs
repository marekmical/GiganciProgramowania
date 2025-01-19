using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace GiganciProgramowania
{
    [TestFixture]
    public class RegistrationTests : PageTest
    {
        #region Common Constants
        private const string RegistrationPageURL = "https://devtest.giganciprogramowania.edu.pl/zapisz-sie";
        private const string NextButtonText = "Dalej";
        private const string AcceptTermsMessage = "Akceptuję regulamin oraz zapoznałem się z polityką prywatności";
        private const string AcceptMarketingMessage = "Wyrażam zgodę na otrzymywanie informacji na temat kursu, na który została zgłoszona chęć uczestnictwa i na powiązane z nim inne usługi świadczone przez szkołę Giganci Programowania oraz na przesyłanie na mój adres e-mail treści marketingowych, w tym w formie newslettera.";
        #endregion

        #region Negative Tests Constants
        private const string FieldRequiredErrorMessage = "Pole jest wymagane";
        private const string CheckBoxRequiredErrorMessage = "To pole jest wymagane";
        private const string FillAllFieldsAlertMessage = "Prosimy uzupełnić wszystkie wymagane pola.";
        private const string IncorrectEmail = "user#example.com";
        private const string IncorrectEmailMessage = "Nieprawidłowy adres e-mail";
        private const string IncorrectPhoneNumber = "12345665";
        private const string IncorrectPhoneNumberMessage = "Niepoprawny numer telefonu. Numer powinien zawierać 9 cyfr, z opcjonalnym kierunkowym +48 lub +380 na początku.";
        #endregion

        #region Positive Tests Constants
        private const string ParentName = "Artur";
        private const string ParentSurname = "Testowy";
        private const string Email = "karolgiganci+fakedata80696@gmail.com";
        private const string PhoneNumber = "123456651";
        private const string ChildYearOfBirth = "2005";
        private const string ChildName = "AMaciej";
        private const string ChildSurname = "ATestowy";
        private const string ZipCode = "26-900";
        #endregion

        [Test]
        /* Verify required fields for first step of polish registration form module */
        public async Task TC01_MissingRequiredFieldsShouldReturnError()
        {

            await Page.GotoAsync(RegistrationPageURL);

            await Page.GetByRole(AriaRole.Button, new() { Name = NextButtonText }).ClickAsync();

            // Check error message for each textbox
            await Expect(Page.GetByText(FieldRequiredErrorMessage, new() { Exact = true }).First).ToBeVisibleAsync();
            await Expect(Page.GetByText(FieldRequiredErrorMessage, new() { Exact = true }).Nth(1)).ToBeVisibleAsync();
            await Expect(Page.GetByText(FieldRequiredErrorMessage, new() { Exact = true }).Nth(2)).ToBeVisibleAsync();
            await Expect(Page.GetByText(FieldRequiredErrorMessage, new() { Exact = true }).Nth(3)).ToBeVisibleAsync();

            // Check error message for each checkbox
            await Expect(Page.GetByText(CheckBoxRequiredErrorMessage).First).ToBeVisibleAsync();
            await Expect(Page.GetByText(CheckBoxRequiredErrorMessage).Nth(1)).ToBeVisibleAsync();

            // Check alert message 
            await Expect(Page.Locator("div").Filter(new() { HasText = FillAllFieldsAlertMessage }).First).ToBeVisibleAsync();

            // Verify we remain on the same page 
            await Expect(Page.GetByText("Wypełnij dane")).ToBeVisibleAsync();
        }

        [Test]
        /* Verify correct error message appears when incorrect email format is provided */
        public async Task TC02_IncorrectMailFormatShouldReturnError()
        {

            await Page.GotoAsync(RegistrationPageURL);

            // Fill email textbox with incorrect email
            await Page.Locator("input[name=\"email\"]").FillAsync(IncorrectEmail);

            await Page.GetByRole(AriaRole.Button, new() { Name = NextButtonText }).ClickAsync();

            // Check incorrect error message is present
            await Expect(Page.GetByText(IncorrectEmailMessage, new() { Exact = true }).First).ToBeVisibleAsync();

            // Check alert message 
            await Expect(Page.Locator("div").Filter(new() { HasText = FillAllFieldsAlertMessage }).First).ToBeVisibleAsync();

            // Verify we remain on the same page 
            await Expect(Page.GetByText("Wypełnij dane")).ToBeVisibleAsync();
        }

        [Test]
        /* Verify correct error message appears when incorrect phone number format is provided */
        public async Task TC03_IncorrectPhoneNumberShouldReturnError()
        {

            await Page.GotoAsync(RegistrationPageURL);

            // Fill phoneNumber textbox with incorrect phone number
            await Page.Locator("input[name=\"phoneNumber\"]").FillAsync(IncorrectPhoneNumber);

            await Page.GetByRole(AriaRole.Button, new() { Name = NextButtonText }).ClickAsync();

            // Check incorrect error message is present
            await Expect(Page.GetByText(IncorrectPhoneNumberMessage, new() { Exact = true }).First).ToBeVisibleAsync();

            // Check alert message 
            await Expect(Page.Locator("div").Filter(new() { HasText = FillAllFieldsAlertMessage }).First).ToBeVisibleAsync();

            // Verify we remain on the same page 
            await Expect(Page.GetByText("Wypełnij dane")).ToBeVisibleAsync();
        }

        [Test]
        /* Verify correct first step form submission when correct data provided */
        public async Task TC04_CorrectRegistrationDataShouldSubmitTheForm()
        {
            await Page.GotoAsync(RegistrationPageURL);

            // Fill personal details with correct data
            await Page.Locator("input[name=\"parentName\"]").FillAsync(ParentName);
            await Page.Locator("input[name=\"email\"]").FillAsync(Email);
            await Page.Locator("input[name=\"phoneNumber\"]").FillAsync(PhoneNumber);
            await Page.Locator("input[name=\"birthYear\"]").FillAsync(ChildYearOfBirth);
            await Page.Locator("label").Filter(new() { HasText = AcceptTermsMessage }).Locator("span").First.ClickAsync();
            await Page.Locator("label").Filter(new() { HasText = AcceptMarketingMessage }).Locator("span").First.ClickAsync();

            await Page.GetByRole(AriaRole.Button, new() { Name = NextButtonText }).ClickAsync();

            // Verify we moved to the second step of the process and the first step was replaced b
            await Page.Locator("#icon-tick").First.IsVisibleAsync();
            await Page.Locator("#icon-tick").Nth(1).IsHiddenAsync();
        }

        [Test]
        /* Verify registration flow for online annual courses */
        public async Task TC05_CorrectRegistrationDataShouldBeAbleToCompleteRegistrationFlow()
        {
            await Page.GotoAsync(RegistrationPageURL);

            // Fill personal details with correct data
            await Page.Locator("input[name=\"parentName\"]").FillAsync(ParentName);
            await Page.Locator("input[name=\"email\"]").FillAsync(Email);
            await Page.Locator("input[name=\"phoneNumber\"]").FillAsync(PhoneNumber);
            await Page.Locator("input[name=\"birthYear\"]").FillAsync(ChildYearOfBirth);
            await Page.Locator("label").Filter(new() { HasText = AcceptTermsMessage }).Locator("span").First.ClickAsync();
            await Page.Locator("label").Filter(new() { HasText = AcceptMarketingMessage }).Locator("span").First.ClickAsync();

            // Click "Dalej"
            await Page.GetByRole(AriaRole.Button, new() { Name = NextButtonText }).ClickAsync();

            // Click "Programowanie"
            await Page.GetByRole(AriaRole.Button, new() { Name = "PROGRAMOWANIE" }).ClickAsync();

            // Click "Online"
            await Page.GetByRole(AriaRole.Button, new() { Name = "Online" }).ClickAsync();

            // Click "Roczne kursy z programowania"
            await Page.GetByRole(AriaRole.Button, new() { Name = "Roczne kursy z programowania" }).ClickAsync();

            // Click "Wybierz" under "Pierwsze kroki w programowaniu (kurs z elementami AI) ONLINE"
            var aiCourse = Page.GetByText("Pierwsze kroki w programowaniu (kurs z elementami AI) ONLINE");
            ClickNearestSelectionButton(aiCourse);

            // Click "Wybierz" under any timetable that contains actual free slots
            var freeTimeSlot = Page.GetByText("Wolnych miejsc").Or(Page.GetByText("Mamy wolne")).First;
            ClickNearestSelectionButton(freeTimeSlot);

            await Page.Locator("input[name=\"student_firstname\"]").FillAsync(ChildName);
            await Page.Locator("input[name=\"student_lastname\"]").FillAsync(ChildSurname);
            await Page.Locator("input[name=\"lastname\"]").FillAsync(ParentSurname);
            await Page.Locator("input[name=\"zip_code\"]").FillAsync(ZipCode);
            await Page.GetByRole(AriaRole.Button, new() { Name = "Zapisz dziecko" }).ClickAsync();


            // Verify we are on the summary page and 5 of the previous steps are replaced by ticks
            const int NoOfCompletedSteps = 5;
            int tickIndex = 0;
            for (tickIndex = 0; tickIndex < NoOfCompletedSteps; tickIndex++)
            {
                await Page.Locator("#icon-tick").Nth(tickIndex).IsVisibleAsync();
            }
            await Page.Locator("#icon-tick").Nth(tickIndex).IsHiddenAsync();
        }

        private async void ClickNearestSelectionButton(ILocator locator)
        {
            ILocator parent = locator.Locator("..");
            ILocator button;
            while (parent != null)
            {
                button = parent.GetByRole(AriaRole.Button, new() { Name = "Wybierz" }).First;
                if (button.IsVisibleAsync().Result == true)
                {
                    await button.ClickAsync();
                    break;
                }
                parent = parent.Locator("..");
            }
        }
    }
}