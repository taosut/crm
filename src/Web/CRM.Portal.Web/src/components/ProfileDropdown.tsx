import React, { useState } from "react";
import { Link } from "react-router-dom";
import { Dropdown, DropdownMenu, DropdownToggle, DropdownItem } from "reactstrap";

import { AuthService } from "services";

interface ProfileDropdownProps {
  profilePic?: string;
  userName?: any;
}

const ProfileMenus = [
  {
    label: "My Account",
    icon: "fe-user",
    redirectTo: "/"
  },
  {
    label: "Settings",
    icon: "fe-settings",
    redirectTo: "/"
  },
  {
    label: "Lock Screen",
    icon: "fe-lock",
    redirectTo: "/"
  },
  {
    label: "Logout",
    icon: "fe-log-out",
    hasDivider: true,
    onClick: () => AuthService.signOut()
  }
];

export default (props: ProfileDropdownProps) => {
  const [dropdownOpen, setDropdownOpen] = useState(false);

  return (
    <Dropdown
      className="notification-list"
      isOpen={dropdownOpen}
      toggle={() => setDropdownOpen(!dropdownOpen)}>
      <DropdownToggle
        data-toggle="dropdown"
        tag="button"
        className="btn btn-link nav-link dropdown-toggle nav-user mr-0 waves-effect waves-light"
        onClick={() => setDropdownOpen(!dropdownOpen)}
        aria-expanded={dropdownOpen}>
        <img src={props.profilePic} className="rounded-circle" alt="user" />
        <span className="pro-user-name ml-1">{props.userName} </span>
      </DropdownToggle>

      <DropdownMenu right className="profile-dropdown">
        <div onClick={() => setDropdownOpen(!dropdownOpen)}>
          <div className="dropdown-header noti-title">
            <h6 className="text-overflow m-0">Welcome !</h6>
          </div>
          {ProfileMenus.map((item, idx) => {
            return (
              <React.Fragment key={idx + "-profile-menu"}>
                {item.hasDivider ? <DropdownItem divider /> : null}
                {item.redirectTo ? (
                  <Link to={item.redirectTo} className="dropdown-item notify-item">
                    <i className={`${item.icon} mr-1`}></i>
                    <span>{item.label}</span>
                  </Link>
                ) : (
                  <button onClick={item.onClick} className="dropdown-item notify-item">
                    <i className={`${item.icon} mr-1`}></i>
                    <span>{item.label}</span>
                  </button>
                )}
              </React.Fragment>
            );
          })}
        </div>
      </DropdownMenu>
    </Dropdown>
  );
};
