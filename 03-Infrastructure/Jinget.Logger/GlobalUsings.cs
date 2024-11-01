﻿global using Jinget.Core.ExtensionMethods;
global using System.Collections.Concurrent;
global using System.Threading;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using Jinget.Core.IOptionTypes.Log;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Newtonsoft.Json;
global using Jinget.Core.Exceptions;
global using Jinget.Logger.Configuration.ElasticSearch;
global using Jinget.Logger.ViewModels;
global using Nest;
global using System.Collections.Generic;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.AspNetCore.Builder;
global using Elasticsearch.Net;
global using Jinget.Logger.Handlers;
global using Jinget.Logger.Handlers.CommandHandlers;
global using Jinget.Logger.Providers.ElasticSearchProvider;
global using Jinget.Logger.Providers.FileProvider;
global using Microsoft.Extensions.Hosting;
global using Jinget.ExceptionHandler.Entities;
global using Jinget.ExceptionHandler.Entities.Log;