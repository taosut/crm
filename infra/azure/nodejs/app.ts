import { ResourceManagement } from "./resource-management";
import { AksManagement } from "./aks-management";

import { AppSetting } from "./model";

const appSetting: AppSetting = require("./values.json");

async function main() {
    await new ResourceManagement().Run(appSetting.resouceGroup.name, appSetting.resouceGroup.parameters);
    await new AksManagement().setupNewCluster(appSetting.resouceGroup.name, appSetting.aks.name, appSetting.aks.parameters);
}

main();