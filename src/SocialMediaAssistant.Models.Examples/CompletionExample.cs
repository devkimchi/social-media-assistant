using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace SocialMediaAssistant.Models.Examples;

/// <summary>
/// This represents the example entity for completion.
/// </summary>
public class CompletionExample : OpenApiExample<string>
{
    /// <inheritdoc />
    public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
    {
        var completion = @"🔧🚗 Exciting News: Join us at the Garage Hack Event! 🚗🔧

Calling all DIY enthusiasts, tinkerers, and innovators! Get ready to rev up your creativity at the Garage Hack Event, where we're turning garages into innovation hubs!

📅 Date: [Insert Date]
🕒 Time: [Insert Time]
📍 Location: [Insert Location]

At Garage Hack, you can:
✅ Unleash Your Creativity: Explore your wildest ideas and bring them to life.
✅ Build Something Amazing: From gadgets to gizmos, we've got the tools and tech you need.
✅ Collaborate and Connect: Network with fellow inventors and tech enthusiasts.
✅ Win Prizes: Compete in fun challenges and win fantastic prizes.
✅ Fuel Your Passion: Learn from expert speakers and gain new skills.

Whether you're a seasoned maker or just curious about the world of DIY, this event is for you! Don't miss out on this incredible opportunity to turn your garage into a hub of innovation.

🎟️ RSVP here: [Link to Registration]
📣 Spread the word and bring your friends!

Get ready to hack, build, and create at the Garage Hack Event! 🛠️ Let's turn our garages into the ultimate playground of innovation. See you there! 👋 #GarageHackEvent #InnovationHub #DIYRevolution";

        this.Examples.Add(OpenApiExampleResolver.Resolve("completion", completion, namingStrategy));

        return this;
    }
}
