using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using NotificationsExtensions.Toasts;
using System;
using Windows.Data.Xml.Dom;
using Windows.Foundation.Metadata;
using Windows.System.Profile;

namespace QuickstartWeatherTileToast
{
    public class NotificationHelper
    {
        public static XmlDocument GenerateToastContent()
        {
            // Start by constructing the visual portion of the toast
            ToastBindingGeneric binding = new ToastBindingGeneric();

            // We'll always have this summary text on our toast notification
            // (it is required that your toast starts with a text element)
            binding.Children.Add(new AdaptiveText()
            {
                Text = "Today will be sunny with a high of 63 and a low of 42."
            });

            // If Adaptive Toast Notifications are supported
            if (IsAdaptiveToastSupported())
            {
                // Use the rich Tile-like visual layout
                binding.Children.Add(new AdaptiveGroup()
                {
                    Children =
                    {
                        GenerateSubgroup("Mon", "Mostly Cloudy.png", 63, 42),
                        GenerateSubgroup("Tue", "Cloudy.png", 57, 38),
                        GenerateSubgroup("Wed", "Sunny.png", 59, 43),
                        GenerateSubgroup("Thu", "Sunny.png", 62, 42),
                        GenerateSubgroup("Fri", "Sunny.png", 71, 66)
                    }
                });
            }

            // Otherwise...
            else
            {
                // We'll just add two simple lines of text
                binding.Children.Add(new AdaptiveText()
                {
                    Text = "Monday ⛅ 63° / 42°"
                });

                binding.Children.Add(new AdaptiveText()
                {
                    Text = "Tuesday ☁ 57° / 38°"
                });
            }

            // Construct the entire notification
            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    // Use our binding from above
                    BindingGeneric = binding,

                    // Set the base URI for the images, so we don't redundantly specify the entire path
                    BaseUri = new Uri("Assets/NotificationAssets/", UriKind.Relative)
                },

                // Include launch string so we know what to open when user clicks toast
                Launch = "action=viewForecast&zip=98008"
            };

            // Return the XmlDocument for the notification
            return content.GetXml();
        }

        private static bool IsAdaptiveToastSupported()
        {
            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                // Desktop and Mobile started supporting adaptive toasts in build 14332
                case "Windows.Mobile":
                case "Windows.Desktop":
                    return GetOSVersion() > new Version(10, 0, 14332, 0);

                // Other device families do not support adaptive toasts
                default:
                    return false;
            }
        }

        private static Version GetOSVersion()
        {
            // The DeviceFamilyVersion is a string, which is actually a ulong number representing the version 
            // https://www.suchan.cz/2015/08/uwp-quick-tip-getting-device-os-and-app-info/ 

            ulong versionAsLong = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);

            ulong v1 = (versionAsLong & 0xFFFF000000000000L) >> 48;
            ulong v2 = (versionAsLong & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (versionAsLong & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (versionAsLong & 0x000000000000FFFFL);

            return new Version((int)v1, (int)v2, (int)v3, (int)v4);
        }

        public static XmlDocument GenerateTileContent()
        {
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileSmall = GenerateTileBindingSmall(),
                    TileMedium = GenerateTileBindingMedium(),
                    TileWide = GenerateTileBindingWide(),
                    TileLarge = GenerateTileBindingLarge(),

                    // Set the base URI for the images, so we don't redundantly specify the entire path
                    BaseUri = new Uri("Assets/NotificationAssets/", UriKind.Relative)
                }
            };

            return content.GetXml();
        }

        private static TileBinding GenerateTileBindingSmall()
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    TextStacking = TileTextStacking.Center,

                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Mon",
                            HintStyle = AdaptiveTextStyle.Body,
                            HintAlign = AdaptiveTextAlign.Center
                        },

                        new AdaptiveText()
                        {
                            Text = "63°",
                            HintStyle = AdaptiveTextStyle.Base,
                            HintAlign = AdaptiveTextAlign.Center
                        }
                    }
                }
            };
        }

        private static TileBinding GenerateTileBindingMedium()
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                GenerateSubgroup("Mon", "Mostly Cloudy.png", 63, 42),
                                GenerateSubgroup("Tue", "Cloudy.png", 57, 38)
                            }
                        }
                    }
                }
            };
        }

        private static TileBinding GenerateTileBindingWide()
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                GenerateSubgroup("Mon", "Mostly Cloudy.png", 63, 42),

                                GenerateSubgroup("Tue", "Cloudy.png", 57, 38),

                                GenerateSubgroup("Wed", "Sunny.png", 59, 43),

                                GenerateSubgroup("Thu", "Sunny.png", 62, 42),

                                GenerateSubgroup("Fri", "Sunny.png", 71, 66)
                            }
                        }
                    }
                }
            };
        }

        private static TileBinding GenerateTileBindingLarge()
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 30,
                                    Children =
                                    {
                                        new AdaptiveImage() { Source = "Mostly Cloudy.png" }
                                    }
                                },

                                new AdaptiveSubgroup()
                                {
                                    Children =
                                    {
                                        new AdaptiveText()
                                        {
                                            Text = "Monday",
                                            HintStyle = AdaptiveTextStyle.Base
                                        },

                                        new AdaptiveText()
                                        {
                                            Text = "63° / 42°"
                                        },

                                        new AdaptiveText()
                                        {
                                            Text = "20% chance of rain",
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        },

                                        new AdaptiveText()
                                        {
                                            Text = "Winds 5 mph NE",
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        }
                                    }
                                }
                            }
                        },

                        // For spacing
                        new AdaptiveText(),

                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                GenerateLargeSubgroup("Tue", "Cloudy.png", 57, 38),

                                GenerateLargeSubgroup("Wed", "Sunny.png", 59, 43),

                                GenerateLargeSubgroup("Thu", "Sunny.png", 62, 42),

                                GenerateLargeSubgroup("Fri", "Sunny.png", 71, 66)
                            }
                        }
                    }
                }
            };
        }

        private static AdaptiveSubgroup GenerateSubgroup(string day, string img, int tempHi, int tempLo)
        {
            return new AdaptiveSubgroup()
            {
                HintWeight = 1,

                Children =
                {
                    // Day
                    new AdaptiveText()
                    {
                        Text = day,
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    // Image
                    new AdaptiveImage()
                    {
                        Source = img,
                        HintRemoveMargin = true
                    },

                    // High temp
                    new AdaptiveText()
                    {
                        Text = tempHi + "°",
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    // Low temp
                    new AdaptiveText()
                    {
                        Text = tempLo + "°",
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
            };
        }

        private static AdaptiveSubgroup GenerateLargeSubgroup(string day, string image, int high, int low)
        {
            // Generate the normal subgroup
            var subgroup = GenerateSubgroup(day, image, high, low);

            // Allow there to be padding around the image
            (subgroup.Children[1] as AdaptiveImage).HintRemoveMargin = null;

            return subgroup;
        }

        private static AdaptiveText GenerateLegacyToastText(string day, string weatherEmoji, int tempHi, int tempLo)
        {
            return new AdaptiveText()
            {
                Text = $"{day} {weatherEmoji} {tempHi}° / {tempLo}°"
            };
        }
    }
}
