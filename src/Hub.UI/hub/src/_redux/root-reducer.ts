import { combineReducers } from "redux";

import authReducer, { loginUser, logoutUser, selectAuthToken, selectUser } from './features/auth/slice';

export const rootReducer = combineReducers({
  auth: authReducer,
});