// Copyright (c) Duende Software. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Newtonsoft.Json.Linq;


namespace IdentityModel.Client {

/// <summary>
/// MTLS endpoint aliases
/// </summary>
public class MtlsEndpointAliases
{
    /// <summary>
    /// The raw JSON
    /// </summary>
    public JToken? Json { get; }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="json"></param>
    public MtlsEndpointAliases(JToken? json)
    {
        Json = json;
    }

    /// <summary>
    /// Returns the token endpoint address
    /// </summary>
    public string? TokenEndpoint => Json?.TryGetString(OidcConstants.Discovery.TokenEndpoint);
        
    /// <summary>
    /// Returns the revocation endpoint address
    /// </summary>
    public string? RevocationEndpoint => Json?.TryGetString(OidcConstants.Discovery.RevocationEndpoint);
        
    /// <summary>
    /// Returns the device authorization endpoint address
    /// </summary>
    public string? DeviceAuthorizationEndpoint => Json?.TryGetString(OidcConstants.Discovery.DeviceAuthorizationEndpoint);
        
    /// <summary>
    /// Returns the introspection endpoint address
    /// </summary>
    public string? IntrospectionEndpoint => Json?.TryGetString(OidcConstants.Discovery.IntrospectionEndpoint);
        
}}