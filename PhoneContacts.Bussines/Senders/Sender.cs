using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;

namespace PhoneContacts.Business.Senders
{
    public static class Sender
    {
        public async static Task<bool> SMS(string numberToSend, string MessageToSend)
        {
            SmsIr smsIr = new SmsIr("SWMe4WZdzNQ5PpIRK1krNS6bpdYy36XdgPuazGkoMgAxxCYlto8Pu04CKwRudZ9x");
            var verificationSendResult = await smsIr.VerifySendAsync(numberToSend, 100000, new VerifySendParameter[] { new VerifySendParameter("Code", MessageToSend) });
            string result = verificationSendResult.Message;

            if (result == "موفق") return true;

            return false;
        }
    }
}
