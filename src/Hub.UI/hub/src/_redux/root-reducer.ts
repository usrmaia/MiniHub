import { combineReducers } from "redux";

import authReducer from "./features/auth/slice";
import handleSideBar from "./features/handleSideBar/slice";
import roleReducer from "./features/role/slice";
import userReducer from "./features/user/slice";

export const rootReducer = combineReducers({
  auth: authReducer,
  handleSideBar: handleSideBar,
  role: roleReducer,
  user: userReducer,
});