﻿using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using static Toggl.Daneel.Tests.UI.Extensions.LoginExtensions;

namespace Toggl.Daneel.Tests.UI
{
    [TestFixture]
    public class LoginTests
    {
        private const string ValidEmail = "susancalvin@psychohistorian.museum";

        private iOSApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp
                .iOS
                .EnableLocalScreenshots()
                .StartApp();

            app.WaitForLoginScreen();
        }

        [Test]
        public void TheNextButtonShowsThePasswordField()
        {
            app.EnterText(ValidEmail);

            app.GoToPasswordScreen();

            app.Screenshot("Login password page.");
        }

        [Test]
        public void TheBackButtonClosesTheLoginViewIfTheEmailFieldIsVisible()
        {
            app.GoBackToOnboardingScreen();

            app.Screenshot("Onboarding last page.");
        }

        [Test]
        public void TheBackButtonShowsTheEmailFieldIfThePasswordFieldIsVisible()
        {
            app.EnterText(ValidEmail);
            app.GoToPasswordScreen();

            app.GoBackToEmailScreen();

            app.Screenshot("Login email page.");
        }
    }
}
