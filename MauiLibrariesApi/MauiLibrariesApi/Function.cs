using System.Net;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MauiLibrariesApi;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayProxyResponse FunctionHandler(ILambdaContext context)
    {
		context.Logger.LogInformation("Get Request\n");

		var serializedLibraryModels = JsonSerializer.Serialize(CreateLibraries());

		var response = new APIGatewayProxyResponse
		{
			StatusCode = (int)HttpStatusCode.OK,
			Body = serializedLibraryModels,
			Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
		};

		return response;
	}

	static List<LibraryModel> CreateLibraries() => new()
	{
		new()
		{
			Title = "Microsoft.Maui",
			Description =
				".NET Multi-platform App UI is a framework for building native device applications spanning mobile, tablet, and desktop",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/microsoft.maui.controls/8.0.3/icon"
		},
		new()
		{
			Title = "CommunityToolkit.Maui",
			Description =
				"The .NET MAUI Community Toolkit is a community-created library that contains .NET MAUI Extensions, Advanced UI/UX Controls, and Behaviors to help make your life as a .NET MAUI developer easier",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/communitytoolkit.maui/5.2.0/icon"
		},
		new()
		{
			Title = "CommunityToolkit.Maui.Markup",
			Description =
				"The .NET MAUI Markup Community Toolkit is a community-created library that contains Fluent C# Extension Methods to easily create your User Interface in C#",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/communitytoolkit.maui.markup/3.2.0/icon"
		},
		new()
		{
			Title = "CommunityToolkit.MVVM",
			Description =
				"This package includes a .NET MVVM library with helpers such as ObservableObject, ObservableRecipient, ObservableValidator, RelayCommand, AsyncRelayCommand, WeakReferenceMessenger, StrongReferenceMessenger and IoC",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/communitytoolkit.mvvm/8.2.0/icon"
		},
		new()
		{
			Title = "Sentry.Maui",
			Description =
				"Bad software is everywhere, and we're tired of it. Sentry is on a mission to help developers write better software faster, so we can get back to enjoying technology",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/sentry.maui/3.33.1/icon"
		},
		new()
		{
			Title = "Esri.ArcGISRuntime.Maui",
			Description =
				"Contains APIs and UI controls for building native mobile and desktop apps with the .NET Multi-platform App UI (.NET MAUI) cross-platform framework",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/esri.arcgisruntime.maui/100.14.1-preview3/icon"
		},
		new()
		{
			Title = "Syncfusion.Maui.Core",
			Description =
				"This package contains .NET MAUI Avatar View, .NET MAUI Badge View, .NET MAUI Busy Indicator, .NET MAUI Effects View, and .NET MAUI Text Input Layout components for .NET MAUI application",
			ImageSource = "https://api.nuget.org/v3-flatcontainer/syncfusion.maui.core/21.2.10/icon"
		},
		new()
		{
			Title = "DotNet.Meteor",
			Description = "A VSCode extension that can run and debug .NET apps (based on Clancey VSCode.Comet)",
			ImageSource =
				"https://nromanov.gallerycdn.vsassets.io/extensions/nromanov/dotnet-meteor/3.0.3/1686392945636/Microsoft.VisualStudio.Services.Icons.Default"
		}
	};
}
