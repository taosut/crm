import { UserManager } from "oidc-client";
import { OidcConfig } from "configs/OidcConfig";
import LoggerService from "./LoggerService";
import { Location } from "history";

class AuthenticationService {
  private userManager: UserManager;

  constructor() {
    this.userManager = new UserManager(OidcConfig);
  }

  get UserManager(): UserManager {
    return this.userManager;
  }

  async authenticateUser(location: Location<any>) {
    if (!this.userManager || !this.userManager.getUser) {
      return;
    }

    let oidcUser = await this.userManager.getUser();

    if (!oidcUser || oidcUser.expired) {
      LoggerService.debug("authenticating user ...");

      // let url = location.location.pathname + (location.location.search || "");
      let url = location.pathname + (location.search || "");
      await this.userManager.signinRedirect({ data: { url } });
    }
  }

  async signOut() {
    if (!this.userManager || !this.userManager.getUser) {
      return;
    }

    let oidcUser = await this.userManager.getUser();
    if (oidcUser) {
      LoggerService.info("Logout user...");
      await this.userManager.signoutRedirect();
    }
  }
}

export default new AuthenticationService();
