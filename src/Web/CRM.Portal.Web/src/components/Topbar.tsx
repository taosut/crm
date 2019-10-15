import React, { useContext } from "react";
import { Link } from "react-router-dom";

import ProfileDropdown from "./ProfileDropdown";
import profilePic from "../assets/img/user.png";
import { AppCtx } from "contexts";

interface TopbarProps {
    rightSidebarToggle: (event: React.MouseEvent<HTMLButtonElement>) => void;
}

export default (props: TopbarProps) => {
    const { state } = useContext(AppCtx);

    return (
        <>
            <div className="navbar-custom">
                <div className="container-fluid">
                    <ul className="list-unstyled topnav-menu float-right mb-0">
                        <li>
                            <ProfileDropdown
                                profilePic={profilePic}
                                userName={state.userLogin ? state.userLogin.userName : ""}
                            />
                        </li>
                        <li className="dropdown notification-list">
                            <button
                                className="btn btn-link nav-link right-bar-toggle waves-effect waves-light"
                                onClick={props.rightSidebarToggle}>
                                <i className="fe-settings noti-icon"></i>
                            </button>
                        </li>
                    </ul>

                    <div className="logo-box">
                        <Link to="/" className="logo text-center">
                            <h4 className="text-white font-weight-bolder mt-3">Contact manager</h4>
                        </Link>
                    </div>
                </div>
            </div>
        </>
    );
};
