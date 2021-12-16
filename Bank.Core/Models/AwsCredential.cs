using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Models
{
    public class AwsCredential
    {
        private readonly AwsOptions _aswsoptions;
        public AwsCredential(AwsOptions awsOptions)
        {
            _aswsoptions = awsOptions;
           
            
            var options = new CredentialProfileOptions
            {
                AccessKey = _aswsoptions.AWSAccessKey,
                SecretKey = _aswsoptions.AWSSecretKey,
            };
            var profile = new CredentialProfile("profile", options);
            profile.Region = RegionEndpoint.USEast1;
            var sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);
        }

        public AWSCredentials GetCredentials()
        {
            var chain = new CredentialProfileStoreChain();
            AWSCredentials result;
            chain.TryGetAWSCredentials("profile", out result);
            return result;
            

        }
    }
}
