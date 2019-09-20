import { ResourceGroup } from "@azure/arm-resources/esm/models";
import { ManagedCluster } from "@azure/arm-containerservice/esm/models";

export interface AppSetting {
    resouceGroup: {
        name: string,
        parameters: ResourceGroup
    }

    aks: {
        name: string,
        parameters: ManagedCluster
    }
}
