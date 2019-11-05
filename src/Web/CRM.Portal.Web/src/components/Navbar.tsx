import React from "react";
import { Link } from "react-router-dom";
import { Collapse } from "reactstrap";

const NavMenuContent = () => {
  return (
    <React.Fragment>
      <ul className="navigation-menu">
        <li className="has-submenu">
          <Link to="/dashboard" className="side-nav-link-ref">
            <i className="mdi mdi-view-dashboard"></i>
            Dashboard
          </Link>
        </li>

        <li className="has-submenu">
          <Link to="/company" className="side-nav-link-ref">
            <i className="mdi mdi-domain"></i>
            Companies
          </Link>
        </li>

        <li className="has-submenu">
          <Link to="/contact" className="side-nav-link-ref">
            <i className="mdi mdi-folder-account"></i>
            Contacts
          </Link>
        </li>

        <li className="has-submenu">
          <Link to="/dashboard" className="side-nav-link-ref">
            <i className="mdi mdi-phone-in-talk"></i>
            Deals
          </Link>
        </li>

        <li className="has-submenu">
          <Link to="/dashboard" className="side-nav-link-ref">
            <i className="mdi mdi-calendar-check"></i>
            Tasks
          </Link>
        </li>

        <li className="has-submenu">
          <Link to="/dashboard" className="side-nav-link-ref">
            <i className="mdi mdi-chart-arc"></i>
            Reports
          </Link>
        </li>
      </ul>
    </React.Fragment>
  );
};

export default () => {
  return (
    <>
      <div className="topbar-menu">
        <div className="container-fluid">
          <Collapse id="navigation" isOpen={true}>
            <NavMenuContent />
            <div className="clearfix"></div>
          </Collapse>
        </div>
      </div>
    </>
  );
};
