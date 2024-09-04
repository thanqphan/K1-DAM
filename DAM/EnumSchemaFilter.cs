namespace DAM;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum = Enum.GetNames(context.Type)
                              .Select(name => new OpenApiString(name))
                              .Cast<IOpenApiAny>()
                              .ToList();
            schema.Type = "string";
            schema.Format = null;
        }
    }
}
