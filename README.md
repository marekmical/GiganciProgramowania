# GiganciProgramowania

Technical Details / Overview:
1. Solution uses Nunit 3 Test Project configuration for tests execution
2. Playwright has been chosen as a web testing engine due to C# and Nunit support, as well as convenience with using codegen to record and analyze testing steps and elements
3. SQL Script, which was a part of the task, is included in "Scripts" directory

How to run / Prerequisites:
1. Ensure Visual Studio 2022 is installed
2. Clone the repository
3. Ensure NodeJS is installed ( https://nodejs.org/en/download )
4. Install playwright browsers using the following command in PowerShell in repository's working directory: npx playwright install
5. Open .sln file in Visual Studio
6. Go to "Test Explorer" view and select "Run all tests in view"

Encountered challenges:
1. In certain scenarios the red alert popup that signals the registration form fields being not satisfied does not display at the first chance. Either a second button click is necessary or previous click on Email textbox is necessary.<br>
Verified manually. Due to this issue TC-01 and TC-03 fail during automatic execution.
