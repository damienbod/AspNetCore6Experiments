﻿using Microsoft.AspNetCore.Builder;

namespace BlazorBffAzureADWithApi.Server
{
    public static class SecurityHeadersDefinitions
    {
        public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev, string idpHost)
        {
            var policy = new HeaderPolicyCollection()
                .AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .AddCrossOriginOpenerPolicy(builder =>
                {
                    builder.SameOrigin();
                })
                .AddCrossOriginResourcePolicy(builder =>
                {
                    builder.SameOrigin();
                })
                .RemoveServerHeader()
                .AddPermissionsPolicy(builder =>
                {
                    builder.AddAccelerometer().None();
                    builder.AddAutoplay().None();
                    builder.AddCamera().None();
                    builder.AddEncryptedMedia().None();
                    builder.AddFullscreen().All();
                    builder.AddGeolocation().None();
                    builder.AddGyroscope().None();
                    builder.AddMagnetometer().None();
                    builder.AddMicrophone().None();
                    builder.AddMidi().None();
                    builder.AddPayment().None();
                    builder.AddPictureInPicture().None();
                    builder.AddSyncXHR().None();
                    builder.AddUsb().None();
                });

            //if (!isDev) // hot reload hack, CSP needs to be almost disabled for this to work
            //{
            policy.AddContentSecurityPolicy(builder =>
                    {
                        builder.AddObjectSrc().None();
                        builder.AddBlockAllMixedContent();
                        builder.AddImgSrc().Self().From("data:");
                        builder.AddFormAction().Self().From(idpHost);
                        builder.AddFontSrc().Self();
                        builder.AddStyleSrc().Self();
                        builder.AddBaseUri().Self();
                        builder.AddFrameAncestors().None();

                        // due to Blazor
                        builder.AddScriptSrc()
                            .Self()
                            .WithHash256("v8v3RKRPmN4odZ1CWM5gw80QKPCCWMcpNeOmimNL2AA=")
                            .UnsafeEval();
                    })
                    .AddCrossOriginEmbedderPolicy(builder => // missing from DEV due to hot reload
                    {
                        builder.RequireCorp();
                    });
                    
                // maxage = one year in seconds
                policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365);
            //}
            //else // if you need to use hot reload
            //{
            //    policy.AddContentSecurityPolicy(builder =>
            //    {
            //        builder.AddObjectSrc().None();
            //        builder.AddBlockAllMixedContent();
            //        builder.AddImgSrc().Self().From("data:");
            //        builder.AddFormAction().Self().From(idpHost);
            //        builder.AddFontSrc().Self();
                    
            //        builder.AddBaseUri().Self();
            //        builder.AddFrameAncestors().None();

            //        // due to Blazor hot reload (DO NOT USE IN PROD)
            //        //builder.AddStyleSrc().Self();
            //        //builder.AddScriptSrc().Self().UnsafeInline().UnsafeEval();
            //    });
            //}

            return policy;
        }
    }
}
