TapIt Windows Phone SDK
=======================
Version 0.1 - This software is currently in development, don't expect it to work yet!

This is the WP8 SDK for the TapIt! mobile ad network. Go to http://tapit.com/ for more details and to sign up.

Usage
---------
The SDK can be downloaded from https://github.com/tapit/TapIt-Windows-SDK/tree/master/TapIt-WP8 . 
Copy the /TapIt-WP8 folders into your Visual studio project. In your project, right click on the references folder and add reference of TapIt-WP8 to your project.

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
_bannerAdView.AnimationTimeInterval = 10;
_bannerAdView.AnimationDuration = 4;

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

Adprompt Usage
-----------------------
AdPrompts are a simple ad unit designed to have a native feel.  The user is given the option to download an app, 
and if they accept, they are taken to the marketplace.

````csharp
// in your xaml.cs file

using TapIt_WP8; // Adding namespace

// Data member
    AlertAdView _tapItAdView;

// initialize Alert ad and add to your page (constructor)
_tapItAdView = new AlertAdView();

// set the alert ad parameters 
_tapItAdView.ZoneId = 1234; // Insert your zone id here

// load the ad
_tapItAdView.Load();

// show the ad
_tapItAdView.Visible = Visibility.Visible;

// hide the ad
_tapItAdView.Visible = Visibility.Collapsed;
````
For complete example, please refer 
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8-TestApp/TapIt-WP8-TestApp/AlertAdPage.xaml.cs 

Callbacks
-------------
For Callback details of WP8 SDK, please refer
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8/TapIt-WP8/AdView.cs 

