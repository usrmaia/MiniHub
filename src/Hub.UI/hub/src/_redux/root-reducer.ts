import { combineReducers } from "redux";

import authReducer from './features/auth/slice';
import roleReducer from './features/role/slice';
import userReducer from './features/user/slice';

export const rootReducer = combineReducers({
  auth: authReducer,
  role: roleReducer,
  user: userReducer,
});