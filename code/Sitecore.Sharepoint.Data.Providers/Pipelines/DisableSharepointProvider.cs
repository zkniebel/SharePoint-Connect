// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableSharepointProvider.cs" company="Sitecore A/S">
//   Copyright (C) 2010 by Sitecore A/S
// </copyright>
// <summary>
//   Defines the DisableSharepointProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Sitecore.Sharepoint.Data.Providers.Pipelines
{
  using System.Linq;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Publishing.Pipelines.PublishItem;

  using SharepointFieldIDs = Sitecore.Sharepoint.Common.FieldIDs;

  /// <summary>
  /// Class uses for publishing Sharepoint items.
  /// </summary>
  public class DisableSharepointProvider
  {
    /// <summary>
    /// This method disable SharepointProvider.
    /// </summary>
    /// <param name="args">The args  .</param>
    public void Disable([NotNull] PublishItemContext args)
    {
      Assert.ArgumentNotNull(args, "args");

      args.CustomData["SharepointDisabler"] = new IntegrationDisabler();
    }

        /// <summary>
        /// Clear IsSharepointItem field for published items and enable SharepointProvider.
        /// </summary>
        /// <param name="args">The args   .</param>
        public void Clear(PublishItemContext args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));
            if (args.VersionToPublish != null)
            {
                Item targetItem = args.PublishHelper.GetTargetItem(args.VersionToPublish.ID);
                if (targetItem != null && targetItem.Fields.FirstOrDefault<Field>((Func<Field, bool>)(field => field.ID == Sitecore.Sharepoint.Common.FieldIDs.IsIntegrationItem)) != null)
                {
                    using (new EditContext(targetItem, false, false))
                        new CheckboxField(targetItem.Fields[Sitecore.Sharepoint.Common.FieldIDs.IsIntegrationItem]).Checked = false;
                }
            }
            if (args.CustomData["SharepointDisabler"] == null)
                return;
            IntegrationDisabler integrationDisabler = args.CustomData["SharepointDisabler"] as IntegrationDisabler;
            if (integrationDisabler == null)
                return;
            args.CustomData["SharepointDisabler"] = (object)null;
            integrationDisabler.Dispose();
        }
    }
}
