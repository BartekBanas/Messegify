import React, { FC } from 'react';
import {Outlet} from "react-router-dom";
import classes from "./Center.module.css";

interface CenterProps {}

export const Center: FC<CenterProps> = () => {
  return (
    <div className={classes.centerContainer}>
        <Outlet/>
    </div>
  );
};