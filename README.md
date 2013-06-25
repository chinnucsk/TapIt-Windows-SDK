TapIt Windows Phone SDK
=======================
Version 0.9 Beta

This is the WP8 SDK for the TapIt! mobile ad network. Go to http://tapit.com/ for more details and to sign up.

Usage
---------
To install, download and unzip the sdk archive(https://github.com/tapit/TapIt-Windows-SDK/raw/master/dist/TapIt-WP8-SDK.zip),
then:

1.  Copy the TapIt-WP8.dll file to your project folder. 
2.	Right click on the references folder in the project.
3.	Click the add reference option.
4.	Browse for TapIt-WP8.dll and click ok.
5.	Project need to add following Device Capabilities in order to use WP8 SDK.
    *	````ID_CAP_IDENTITY_DEVICE````
    *	````ID_CAP_LOCATION````



Banner Usage
-------------------
````csharp
// in your xaml.cs file

using TapIt_WP8; // Adding namespace


// Data member
BannerAdView _bannerAdView;


// initialize banner and add to your page (constructor)
_bannerAdView = new BannerAdView();
_bannerAdView.ViewControl.SetValue (Grid.RowProperty, 2);
ContentPanel.Children.Add (_bannerAdView.ViewControl);

// set the banner ad parameters and kick off banner rotation!
_bannerAdView.ZoneId = 1234; // Insert your zone id here

// load the ad
_bannerAdView.LoadAndNavigate();

// show the ad
_bannerAdView.Visible = Visibility.Visible;

// hide the ad
_bannerAdView.Visible = Visibility.Collapsed;
````
For complete example, please refer
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8-TestApp/TapIt-WP8-TestApp/BannerAdPage.xaml.cs 


Interstitial Usage
-----------------------
````csharp
// in your xaml.cs file

using TapIt_WP8; // Adding namespace


// Data member
InterstitialAdView _interstitialAdView;


// initialize interstitial and add to your page (constructor)
_interstitialAdView = new InterstitialAdView();
ContentPanel.Children.Add(_interstitialAdView.ViewControl);

// set the Interstitial ad parameters	
_interstitialAdView.ZoneId = 1234; // Insert your zone id here

// load the ad
_interstitialAdView.LoadAndNavigate();

// show the ad
_interstitialAdView.Visible = Visibility.Visible;

// hide the ad
_interstitialAdView.Visible = Visibility.Collapsed;
````
For complete example, please refer 
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8-TestApp/TapIt-WP8-TestApp/InterstitialAdPage.xaml.cs 

AdPrompt Usage
-----------------------
AdPrompts are a simple ad unit designed to have a native feel.  The user is given the option to download an app, 
and if they accept, they are taken to the marketplace.

````csharp
// in your xaml.cs file

using TapIt_WP8; // Adding namespace

// Data member
AdPromptView _tapItAdView;

// initialize AdPrompt and add to your page (constructor)
_tapItAdView = new AdPromptView();

// set the AdPrompt parameters 
_tapItAdView.ZoneId = 1234; // Insert your zone id here

// load the ad
_tapItAdView.Load();

// show the ad
_tapItAdView.Visible = Visibility.Visible;

// hide the ad
_tapItAdView.Visible = Visibility.Collapsed;
````
For complete example, please refer 
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8-TestApp/TapIt-WP8-TestApp/AdPromptPage.xaml.cs 

Callbacks
-------------
For Callback details of WP8 SDK, please refer
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8/TapIt-WP8/AdView.cs 

