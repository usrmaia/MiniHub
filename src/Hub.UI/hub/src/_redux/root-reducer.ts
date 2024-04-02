import { combineReducers } from "redux";

import { authReducer } from "./features/auth/auth.reducer";

export const rootReducer = combineReducers({
  auth: authReducer,
});