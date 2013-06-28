TapIt Windows Phone SDK
=======================
Version 1.0.0

This is the WP8 SDK for the TapIt! mobile ad network. Go to http://tapit.com/ for more details and to sign up.

Usage
---------
To install, download and unzip the sdk archive(https://github.com/tapit/TapIt-Windows-SDK/raw/master/dist/TapIt-WP8-SDK.zip),
then:

1.  Copy the TapIt-WP8.dll file to your project folder. 
2.   Right click on the references folder in the project.
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

// add the event handlers
_bannerAdView.ControlLoaded += _bannerAdView_controlLoaded;
_bannerAdView.ContentLoaded += _bannerAdView_contentLoaded        
_bannerAdView.ErrorEvent += _bannerAdView_errorEvent;       
_bannerAdView.Navigating += _bannerAdView_navigating;          
_bannerAdView.Navigated += _bannerAdView_navigated;     
_bannerAdView.NavigationFailed += _bannerAdView_navigationFailed;

// handle the orientation change
_bannerAdView.DeviceOrientationChanged(e.Orientation);

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
LayoutRoot.Children.Add(_interstitialAdView.ViewControl);

// set the Interstitial ad parameters	
_interstitialAdView.ZoneId = 1234; // Insert your zone id here

// load the ad
_interstitialAdView.LoadAndNavigate();

// show the ad
_interstitialAdView.Visible = Visibility.Visible;

// hide the ad
_interstitialAdView.Visible = Visibility.Collapsed;

// add the event handlers
 _interstitialAdView.ControlLoaded += _interstitialAdView_ControlLoaded;     
 _interstitialAdView.ContentLoaded += interstitialAdView_LoadCompleted;         
 _interstitialAdView.ErrorEvent += interstitialAdView_ErrorEvent;         
 _interstitialAdView.Navigating += interstitialAdView_navigating;         
 _interstitialAdView.Navigated += interstitialAdView_navigated;       
 _interstitialAdView.NavigationFailed += interstitialAdView_navigationFailed;

// handle the orientation change
_interstitialAdView.DeviceOrientationChanged(e.Orientation);

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

// handle the orientation change
_AdPromptView.DeviceOrientationChanged(e.Orientation);

// Handle the back key press event to remove the Adprompt.
// On back key press event call following method.
_AdPromptView.OnBackKeypressed(e);


````
For complete example, please refer 
https://github.com/tapit/TapIt-Windows-SDK/blob/master/TapIt-WP8-TestApp/TapIt-WP8-TestApp/AdPromptPage.xaml.cs 

State management
-------------------

The Windows Phone execution model allows only one app to run in the foreground at a time.
when the user switches away from an app, it is either suspended or terminated,
depending on the context and the way that the user navigated away.
The Windows Phone application model provides a set of events and related APIs that allow your 
app to handle activation and deactivation in a way that provides a consistent and intuitive user experience.

In the SDK the AdView contents are lost when the application gets deactivated.so on App activation
The contents need to be restored.


// handle the App activation / deactivation

//for banner ad

_bannerAdView.AppActivated(); 

_bannerAdView.AppDeactivated();

//for interstitial ad

_interstitialAdView.AppActivated();

_interstitialAdView.AppDeactivated();

