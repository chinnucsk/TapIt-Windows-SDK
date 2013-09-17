TapIt Windows 8 SDK
=======================
Version 1.0.0

This is the Windows 8 SDK for the TapIt! mobile ad network. Go to http://tapit.com/  for more details and to sign up.

Usage
---------
To install, download and unzip the sdk archive(https://github.com/tapit/TapIt-Windows-SDK/raw/master/dist/TapIt-Windows-SDK.zip),
then:
 
1.    Copy the TapIt-Win8.dll file to your project folder.
2.    Right click on the references folder in the project.
3.    Click the add reference option.
4.    Browse for TapIt-Win8.dll and click ok.
5.    Edit the App Manifest to add following Device Capabilities in order to use Windows 8 SDK.
    * ````Document Library````
    * ````Internet(Client)````
    * ````Location````
6.    Edit the App manifest to add Declarations. Add “File Type Assocation” from “Available Declerations”.

Edit the following sections in properties.
·       Edit flags

Check the always Unsafe checkbox.
·       Supported file type

File Type : .tapit


Banner Usage
-------------------
````csharp
// in your xaml.cs file

using TapIt_Win8; // Adding namespace

// Data member
BannerAdView _bannerAdView;

// initialize banner and add to your page (constructor)
_bannerAdView = new BannerAdView();
_bannerAdView.ViewControl.SetValue (Grid.RowProperty, 2);
ContentPanel.Children.Add (_bannerAdView.ViewControl);
 
// set the banner ad parameters
_bannerAdView.ZoneId = 1234; // Insert your zone id here

// set the ad size
_bannerAdView.SetBannerAdSize(BannerAdView.BannerAdtype.MediumRect);

// load the ad
_bannerAdView.Load();

// show the ad
_bannerAdView.Visible = Visibility.Visible;

// hide the ad
_bannerAdView.Visible = Visibility.Collapsed;

// add the event handlers
_bannerAdView.ControlLoaded += _bannerAdView_controlLoaded;
_bannerAdView.ContentLoaded += _bannerAdView_contentLoaded;
_bannerAdView.ErrorEvent += _bannerAdView_errorEvent;
_bannerAdView.NavigationFailed += _bannerAdView_NavigationFailed;
````

For complete example, please refer to 
https://github.com/tapit/TapIt-Windows-SDK/blob/develop/TapIt-Win8-TestApp/TapIt-Win8-TestApp/BannerAdPage.xaml.cs


Interstitial Usage
-----------------------
````csharp
// in your xaml.cs file
 
using TapIt_Win8; // Adding namespace

// Data member
InterstitialAdView _interstitialAdView;

// initialize interstitial and add to your page (constructor)
_interstitialAdView = new InterstitialAdView();
LayoutRoot.Children.Add(_interstitialAdView.ViewControl);

// set the Interstitial ad parameters    
_interstitialAdView.ZoneId = 1234; // Insert your zone id here

// load the ad
_interstitialAdView.load(); 

// show the ad
_interstitialAdView.Visible = Visibility.Visible;

// hide the ad
_interstitialAdView.Visible = Visibility.Collapsed; 

// add the event handlers
_interstitialAdView.ControlLoaded += _interstitialAdView_ControlLoaded;            _interstitialAdView.ContentLoaded += _interstitialAdView_ContentLoaded;            _interstitialAdView.ErrorEvent += _interstitialAdView_ErrorEvent;            _interstitialAdView.NavigationFailed += _interstitialAdView_NavigationFailed;
````

For complete example, please refer to
https://github.com/tapit/TapIt-Windows-SDK/blob/develop/TapIt-Win8-TestApp/TapIt-Win8-TestApp/InterstitialAdPage.xaml.cs

 

AdPrompt Usage
-----------------------
AdPrompts are a simple ad unit designed to have a native feel.  The user is given the option to download an app,
and if they accept, they are taken to the Windows Store.

````csharp
// in your xaml.cs file

using TapIt_Win8; // Adding namespace

// Data member
AdPromptView _AdPromptView;

// initialize AdPrompt and add to your page (constructor)
_AdPromptView = new AdPromptView();
LayoutRoot.Children.Add(_AdPromptView.ViewControl);

// set the AdPrompt parameters
_AdPromptView.ZoneId = 1234; // Insert your zone id here

// load the ad
_AdPromptView.Load();

// show the ad
_AdPromptView.Visible = Visibility.Visible;

// hide the ad
_ AdPromptView.Visible = Visibility.Collapsed;

// add the event handlers
_AdPromptView.ControlLoaded += _AdPromptView_loaded;
_AdPromptView.ContentLoaded += _AdPromptView_LoadCompleted;
_AdPromptView.ErrorEvent += _AdPromptView_ErrorEvent;
````

For complete example, please refer to
https://github.com/tapit/TapIt-Windows-SDK/blob/develop/TapIt-Win8-TestApp/TapIt-Win8-TestApp/AdPromptPage.xaml.cs