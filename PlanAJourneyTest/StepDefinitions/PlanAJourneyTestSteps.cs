using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

using TechTalk.SpecFlow;

namespace PlanAJourney.PlanAJourneyTest.StepDefinitions
{
[Binding]
public class PlanAJourneyTestFeatureSteps : IDisposable
{
private String searchKeyword;
 
private ChromeDriver chromeDriver;
private string fromLocation ="";
private string toLocation = "";

public PlanAJourneyTestFeatureSteps() => chromeDriver = new ChromeDriver();
 
[Given(@"I have navigated to TfL website")]
public void GivenIHaveNavigatedToTfLWebsite()
{
chromeDriver.Navigate().GoToUrl("https://www.tfl.gov.uk");
Assert.IsTrue(chromeDriver.Title.Contains("Transport for London"));
chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

// Accept Cookies
chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3); 

chromeDriver.FindElement(By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll")).Click() ;

// Click Done button
String javascript = "endCookieProcess(); return false;";   
IWebDriver  driver = (IWebDriver )  chromeDriver; 
IJavaScriptExecutor  jsExecutor = (IJavaScriptExecutor ) driver;        
jsExecutor.ExecuteScript(javascript);  




}
 
[Given(@"I go to my recent journey plans")]
public void WhenIGoToMyRecentJourneyPlans()
{
chromeDriver.FindElement(By.ClassName("tfl-name")).Click();
chromeDriver.FindElement(By.LinkText("Recents")).Click();  
}

[Then(@"I should see my recent journey from (.*) to (.*)")]
public void ThenIShouldSeeMyRecentJourney(string from, string to)
{
    //It doesn't cache the recents although all cookies are enabled
// Assert.True(chromeDriver.FindElement(By.LinkText(from+" to "+to+ " ")).Displayed);


Assert.True(chromeDriver.FindElement(By.CssSelector("#jp-recent-content-home- > p:nth-child(1)")).Text.Contains("no recent"));

}
[Given(@"I plan my journey from (.*)")]
public void GivenIPlanMyJourneyFrom(String fromLocation)
{
this.fromLocation= fromLocation;
chromeDriver.FindElement(By.Id("InputFrom")).SendKeys(fromLocation);
}
 
[Given(@"to (.*)")]
public void GivenTo(string toLocation)
{
this.toLocation = toLocation;
chromeDriver.FindElement(By.Id("InputTo")).SendKeys(toLocation);
}
 
[Given(@"I change the time")]
public void GivenIChangeTheTime()
{
chromeDriver.FindElement(By.ClassName("change-departure-time")).Click();

}

[Then(@"I should see change time link diplays Arriving option")]
public void IShouldSeeChangeTimeLinkDiplaysArrivingOption()
{
Assert.True(chromeDriver.FindElement(By.CssSelector("#plan-a-journey > fieldset > div.time-options.clearfix.change-time > div.change-time-options > div:nth-child(1) > ul > li:nth-child(2) > label")).Displayed);

}

[Given(@"I select an arriving time")]
public void GivenISelectAnArrivingTime()
{
chromeDriver.FindElement(By.CssSelector("#plan-a-journey > fieldset > div.time-options.clearfix.change-time > div.change-time-options > div:nth-child(1) > ul > li:nth-child(2) > label")).Click();

// Select the same time tomorrow
chromeDriver.FindElement(By.Id("Date")).SendKeys("Tomorrow");

}

[Given(@"I edit my journey")]
public void GivenIEditMyJourney()
{
chromeDriver.FindElement(By.ClassName("edit-journey")).Click();

// Select the same time tomorrow
chromeDriver.FindElement(By.Id("Date")).SendKeys("Tomorrow");

}

[When(@"I plan my journey")]
public void WhenIPlanMyJourney()
{
chromeDriver.FindElement(By.Id("plan-journey-button")).Click() ;

}


 
[Then(@"I should see my journey plan in the Journey results page")]
public void ThenIShouldSeeMyJourneyPlanInTheJourneyResultsPage()
{
// Assert the results page
Assert.IsTrue(chromeDriver.FindElement(By.ClassName("jp-results-headline")).Text.Contains("Journey results"));
Assert.IsTrue(chromeDriver.FindElement(By.CssSelector("#plan-a-journey > div.journey-result-summary > div.from-to-wrapper > div:nth-child(1) > span.notranslate > strong")).Text.Contains(fromLocation));
Assert.IsTrue(chromeDriver.FindElement(By.CssSelector("#plan-a-journey > div.journey-result-summary > div.from-to-wrapper > div:nth-child(2) > span.notranslate > strong")).Text.Contains(toLocation));


// Wait for the planned journey
var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(30));
wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("jp-result-transport")));

// Assert the fastest result is displayed
Assert.IsTrue(chromeDriver.FindElement(By.ClassName("jp-result-transport")).Text.Contains("Fastest by public transport"));
// Assert the details button of the fastest journey
Assert.IsTrue(chromeDriver.FindElement(By.LinkText("View details")).Displayed );
// Assert the Map view button of the fastest journey
Assert.IsTrue(chromeDriver.FindElement(By.LinkText("Map view")).Displayed );

}

[Then(@"I should see my journey is not planned in the Journey results page")]
public void ThenIShouldSeeMyJourneyIsNotPlannedInTheJourneyResultsPage()
{
// Assert the results page
Assert.IsTrue(chromeDriver.FindElement(By.ClassName("jp-results-headline")).Text.Contains("Journey results"));
Assert.IsTrue(chromeDriver.FindElement(By.CssSelector("#plan-a-journey > div.journey-result-summary > div.from-to-wrapper > div:nth-child(1) > span.notranslate > strong")).Text.Contains(fromLocation));
Assert.IsTrue(chromeDriver.FindElement(By.CssSelector("#plan-a-journey > div.journey-result-summary > div.from-to-wrapper > div:nth-child(2) > span.notranslate > strong")).Text.Contains(toLocation));


// Wait for the planned journey
var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(30));
wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#full-width-content > div > div:nth-child(8) > div > div > ul > li")));

// Assert the invalid journey plan error message is displayed
Assert.IsTrue(chromeDriver.FindElement(By.CssSelector("#full-width-content > div > div:nth-child(8) > div > div > ul > li")).Text.Contains("Journey planner could not find any results to your search. Please try again"));

}

[Then(@"I should see location validation errors on the form")]
public void ThenIShouldSeeLocationValidationErrorsOnTheForm()
{
// Assert the location validation errors
Assert.IsTrue(chromeDriver.FindElement(By.Id("InputFrom-error")).Text.Contains("The From field is required."));
Assert.IsTrue(chromeDriver.FindElement(By.Id("InputTo-error")).Text.Contains("The To field is required."));

}
  
 
public void Dispose()
{
if(chromeDriver != null)
{
chromeDriver.Dispose();
chromeDriver = null;
}
}
}
}