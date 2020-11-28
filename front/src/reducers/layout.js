const SET_REDIRECT = 'layout/SET_REDIRECTT'

const initialState = {
  redirect: null,
}

export default function (state = initialState, action) {
  switch (action.type) {
    case SET_REDIRECT:
      return {
        ...state,
        redirect: action.path
          ? {
            path: action.path,
            push: action.push
          }
          : null
      }
    default:
      return state
  }
}

export const setRedirect = function (path, push = false) {
  return {
    type: SET_REDIRECT,
    path,
    push
  }
}