using NHapi.Base.Parser;
using NHapi.Model.V24.Group;
using NHapi.Model.V24.Message;
using System;
using HapiModel = NHapi.Base.Model;

namespace HL7Base64Decoder.ProcessingLogic
{
    internal class HL7Handler
    {
        private ORM_O01 orm001;

        public HL7Handler(string fileText)
        {
            PipeParser parser = new PipeParser();

            HapiModel.IMessage m = parser.Parse(fileText);

            orm001 = m as ORM_O01;
        }

        internal byte[] GetPDFStr()
        {
            byte[] decoded = null;

            int obxRepeats = orm001.GetORDER(0).ORDER_DETAIL.OBSERVATIONRepetitionsUsed;
            foreach (ORM_O01_OBSERVATION obs in orm001.GetORDER(0).ORDER_DETAIL.OBSERVATIONs)
            {
                if (obs.OBX.ObservationIdentifier.Identifier.Value == "IEMR_ORDER_INFO")
                {
                    var base64Str = ((NHapi.Model.V24.Datatype.ED)obs.OBX.GetObservationValue(0).Data).Data.Value;

                    decoded = Convert.FromBase64String(base64Str);
                }
            }

            return decoded;
        }

        internal string getMSH10()
        {
            return orm001.MSH.MessageControlID.Value;
        }
    }
}