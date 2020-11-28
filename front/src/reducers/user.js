const SET_USER = 'user/SET_USER'

const initialState = null

export default function (state = initialState, action) {
  switch (action.type) {
    case SET_USER:
      return action.user || null
    default:
      return state
  }
}

export const setUser = function (user) {
  localStorage.setItem('user', user ? JSON.stringify(user) : '')

  return {
    type: SET_USER,
    user
  }
}
