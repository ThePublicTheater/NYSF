using System;
using Nysf.Types;
using System.Data;

namespace Nysf.Tessitura
{
    public class Performance
    {
        #region Variables

        private int id; // The performance / inventory number
        private string name; // The name of the performance
        private int productionId; // The Tess. "production-season" number
        private int venueId; // The Tess. "facility" number
        private DataTable webContent;

        #endregion

        #region Properties

        /// <summary>
        ///     The performance's unique ID number ("perf_no" / "inv_no" in Tessitura).
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "The performance ID must be positive (or zero).");
                id = value;
            }
        }

        /// <summary>
        ///     The performance's name ("description" in Tessitura).
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("The performance name cannot be null.");
                if (value.Length > 30)
                    throw new ArgumentException(
                        "The performance name may not contain more than 30 characters.");
                name = value;
            }
        }

        /// <summary>
        ///     The performance's production ID number ("production-season" in Tessitura).
        /// </summary>
        public int ProductionId
        {
            get
            {
                return productionId;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "The production ID must be positive (or zero).");
                productionId = value;
            }
        }

        /// <summary>
        ///     The ID number of the performance's venue ("facility" in Tessitura).
        /// </summary>
        public int VenueId
        {
            get
            {
                return venueId;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "The venue ID must be positive (or zero).");
                venueId = value;
            }
        }
        public DataTable WebContent
        {
            get
            {
                return webContent;
            }
            set
            {
                webContent = value;
            }
        }

        /// <summary>
        ///     The organization under which the performance is produced.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        ///     The date and time that the performance begins.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     The name of the performance's venue.
        /// </summary>
        public string VenueName
        {
            get
            {
                string venueName = null;
                Venue[] venues = WebClient.GetVenues();
                foreach (Venue venue in venues)
                {
                    if (venue.Id == VenueId)
                    {
                        venueName = venue.Name;
                        break;
                    }
                }
                return venueName;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Instantiates a new Performance object.
        /// </summary>
        /// <param name="id">
        ///     The Tessitura "perf_no" / "inv_no" of the performance.
        /// </param>
        /// <param name="name">
        ///     The Tessitura "description" of the performance.
        /// </param>
        /// <param name="productionId">
        ///     The Tessitura "production-season" of the performance.
        /// </param>
        /// <param name="org">
        ///     The organization under which the performance is produced.
        /// </param>
        /// <param name="time">
        ///     The start date and time of the performance.
        /// </param>
        /// <param name="venueId">
        ///     The ID number of the performance's venue.
        /// </param>
        public Performance(int id, string name, int productionId, Organization org, DateTime time,
            int venueId)
        {
            Id = id;
            Name = name;
            ProductionId = productionId;
            Organization = org;
            StartTime = time;
            VenueId = venueId;
        }
        public Performance(int id, string name, int productionId, Organization org, DateTime time,
            int venueId, DataTable webContent)
        {
            Id = id;
            Name = name;
            ProductionId = productionId;
            Organization = org;
            StartTime = time;
            VenueId = venueId;
            if (verifyWebContentSchema(webContent))
                this.webContent = webContent;
            else
                throw new Exception("The web content schema passed is not the schema expected. See Tessitura SOAP api for correct schema.");
        }

        /// <summary>
        ///     Instantiates a new Performance object.
        /// </summary>
        /// <param name="id">
        ///     The Tessitura "perf_no" / "inv_no" of the performance.
        /// </param>
        /// <param name="name">
        ///     The Tessitura "description" of the performance.
        /// </param>
        /// <param name="productionId">
        ///     The Tessitura "production-season" of the performance.
        /// </param>
        /// <param name="perfType">
        ///     The Tessitura "perf_type" / "prod_type" of the performance (from TR_PERF_TYPE).
        /// </param>
        /// <param name="time">
        ///     The start date and time of the performance.
        /// </param>
        public Performance(int id, string name, int productionId, short perfType, DateTime time,
            int venueId) : this(id, name, productionId, ConvertPerfTypeToOrganization(perfType),
            time, venueId) {}

        public Performance(int id, string name, int productionId, short perfType, DateTime time,
            int venueId, DataTable wc)
            : this(id, name, productionId, ConvertPerfTypeToOrganization(perfType),
                time, venueId) {
                    if (verifyWebContentSchema(wc))
                        webContent = wc;
                    else
                        throw new Exception("The web content schema passed is not the schema expected. See Tessitura SOAP api for correct schema.");
        }

        /// <summary>
        ///     Discerns a NYSF suborganization from a Tessitura "perf_type".
        /// </summary>
        /// <param name="perfType">
        ///     The performance / production type from Tessitura (TR_PERF_TYPE).
        /// </param>
        public static Organization ConvertPerfTypeToOrganization(short perfType)
        {
            Organization tempOrg;
            switch (perfType)
            {
                case 2: // TR_PERF_TYPE "description": Public Theater
                    tempOrg = Organization.PublicTheater;
                    break;
                case 4: // TR_PERF_TYPE "description": Joe's Pub
                    tempOrg = Organization.JoesPub;
                    break;
                case 5: // TR_PERF_TYPE "description": Joe's Pub LLC
                    tempOrg = Organization.JoesPub;
                    break;
                case 6: // TR_PERF_TYPE "description": Under the Radar
                    tempOrg = Organization.PublicTheater;
                    break;
                case 8: // TR_PERF_TYPE "description": Special Events
                    tempOrg = Organization.PublicTheater;
                    break;
                case 9: // TR_PERF_TYPE "description": Delacorte Theater
                    tempOrg = Organization.ShakespeareInThePark;
                    break;
                default:
                    tempOrg = Organization.Other;
                    break;
            }
            return tempOrg;
        }
        private bool verifyWebContentSchema(DataTable webContent)
        {
            string[] columnNames = { "orig_inv_no", "inv_no", "inv_type", "content_type", "content_type_desc", "content_value" };
            if (webContent.Columns.Count != 6)
                return false;
            else
                for (int i = 0; i < 6; i++)
                    if (webContent.Columns[i].ColumnName != columnNames[i])
                        return false;
            return true;

        }

        #endregion
    }
}