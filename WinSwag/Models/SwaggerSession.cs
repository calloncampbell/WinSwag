﻿using Newtonsoft.Json;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinSwag.Models.Arguments;
using WinSwag.ViewModels;

namespace WinSwag.Models
{
    public class SwaggerSession
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string SwaggerDocumentJson { get; set; }

        public Dictionary<string, Dictionary<string, SwaggerArgument>> Arguments { get; set; }

        public static SwaggerSession FromViewModel(SwaggerSpecificationViewModel vm)
        {
            return new SwaggerSession
            {
                SwaggerDocumentJson = vm.Model.ToJson(),
                Arguments = vm.OperationGroups
                    .SelectMany(g => g)
                    .ToDictionary(
                        op => op.Model.Operation.OperationId,
                        op => op.Arguments.ToDictionary(p => p.Parameter.Name, p => p))
            };
        }

        public static async Task<SwaggerSpecificationViewModel> ToViewModelAsync(SwaggerSession session)
        {
            var doc = await SwaggerDocument.FromJsonAsync(session.SwaggerDocumentJson);
            var vm = new SwaggerSpecificationViewModel(doc);

            foreach (var storedOp in session.Arguments)
            {
                var operation = vm.OperationGroups
                    .SelectMany(g => g)
                    .FirstOrDefault(op => op.Model.Operation.OperationId == storedOp.Key);

                if (operation != null)
                {
                    foreach (var storedArg in storedOp.Value)
                    {
                        var parameter = operation.Arguments.FirstOrDefault(p => p.Parameter.Name == storedArg.Key);
                        //if (parameter != null)
                        //    parameter.Value = storedArg.Value;
                    }
                }
            }

            return vm;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static SwaggerSession FromJson(string json)
        {
            return JsonConvert.DeserializeObject<SwaggerSession>(json);
        }
    }
}
