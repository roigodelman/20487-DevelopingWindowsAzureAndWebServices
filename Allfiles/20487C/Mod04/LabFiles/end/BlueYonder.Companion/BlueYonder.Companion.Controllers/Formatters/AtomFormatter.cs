using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BlueYonder.Companion.Entities;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;


namespace BlueYonder.Companion.Controllers.Formatters
{
    public class AtomFormatter : MediaTypeFormatter
    {
        public AtomFormatter()
        {
            SupportedMediaTypes.Add(new
               MediaTypeHeaderValue
               ("application/atom+xml"));
        }

        private HttpRequestMessage _request;

        public AtomFormatter(HttpRequestMessage request)
        {
            _request = request;
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }
        public override bool CanWriteType(Type type)
        {
            return type == typeof(FlightWithSchedulesDTO);
        }
        private SyndicationFeed CreateFeed(FlightWithSchedulesDTO flight)
        {
            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent(string.Format("Blue Yonder flight {0}", flight.FlightNumber))
            };
            var items = from s in flight.Schedules
                        select new SyndicationItem
                        {
                            Title = new TextSyndicationContent(
                                String.Format("Flight {0} {1}", flight.FlightNumber, s.Departure.ToString("MMMM dd, yyyy"))),
                            Id = flight.FlightNumber,
                            BaseUri = new Uri(_request.RequestUri,
                                string.Format("{0}/{1}", _request.RequestUri.AbsolutePath, flight.FlightNumber)),
                        };
            feed.Items = items;
            return feed;


        }
        public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            // creating a System.ServiceModel.Syndication.SyndicationFeed

            var feed = CreateFeed(value as FlightWithSchedulesDTO);
            return Task.Run(() =>
            {
                using (var writer = XmlWriter.Create(writeStream))
                {
                    Atom10FeedFormatter atomformatter = new Atom10FeedFormatter(feed);
                    atomformatter.WriteTo(writer);
                }
            });


        }
        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            return new AtomFormatter(request);
        }

    }
}
