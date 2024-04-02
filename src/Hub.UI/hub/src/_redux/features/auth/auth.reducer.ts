const initialState = {
  user: null,
};

export const authReducer = (state = initialState, action: { type: any; payload: any; }) => {
  switch (action.type) {
    case "auth/login":
      return {
        ...state,
        user: action.payload,
      };
    case "auth/logout":
      return {
        ...state,
        user: null,
      };
    default:
      return state;
  }
};