namespace StateControlSystem.Enums
{
    public static class ServiceResponseMessage
    {
        public static string ApprovedMessage = "Fatura onaylandı.";
        public static string RejectedMessage = "Hatalı imza.";
        public static string BlockedMessage = "Bu faturaya ait art arda 2 red cevabı alındı. Manuel inceleme gerekiyor.";
    }
}
