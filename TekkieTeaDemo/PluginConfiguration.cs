namespace TekkieTeaDemo
{
    public static class PluginConfiguration
    {
        // Blob Storage
        public static string AccountName { get; set; }
        public static string AccountKey { get; set; }
        public static string BlobContainer { get; set; }

        // Table Storage
        public static string AWSSecretKey { get; set; }
        public static string AWSAccessKey { get; set; }
        public static string AWSRegion { get; set; }
        public static string TableName { get; set; }

        public static void Initialise() {
			// Blob Storage
			AccountName = "accountname";
            AccountKey = "accountkey";
            BlobContainer = "blobcontainer";

            // AWS DynamoDB
            AWSSecretKey = "awssecretkey";
            AWSAccessKey = "awsaccesskey";
            AWSRegion = "eu-west-1";
            TableName = "tablename";
        }
	}
}
