import {
  fetchUsers,
  fetchWorkers,
  fetchWorkplaces,
} from '../services/workplaces'
import { fetchTasks, fetchTaskDetails } from '../services/task'

const SET_WORK_PLACES = 'data/SET_WORK_PLACES'
const SET_USERS = 'data/SET_USERS'
const SET_WORKERS = 'data/SET_WORKERS'
const SET_TASK_FORM = 'data/SET_TASK_FORM'
const SET_TASKS = 'data/SET_TASKS'
const SET_TASK_DETAILS = 'data/SET_TASK_DETAILS'
const ADD_COMMENT = 'data/ADD_COMMENT'
const CLEAR_ALL = 'data/CLEAR_ALL'

const initialState = {
  workplaces: undefined,
  users: undefined,
  task: {}
}

export default function (state = initialState, action) {
  switch (action.type) {
    case SET_WORK_PLACES:
      return {
        ...state,
        workplaces: action.data
      }
    
    case SET_USERS: 
      return {
        ...state,
        users: action.data
      }
      
    case SET_TASK_FORM:
      return {
        ...state,
        task: action.data
      }
      
    case SET_TASKS: 
      return {
        ...state,
        [action.page]: action.data
      }
      
    case SET_WORKERS:
      return {
        ...state,
        workers: action.data
      }
      
    case SET_TASK_DETAILS:
      return {
        ...state,
        details: action.data
      }
      
    case ADD_COMMENT:
      return {
        ...state,
        details: {
          ...state.details,
          comments: [
            ...((state.details && state.details.comments) || []),
            action.comment
          ]
        }
      }
      
    case CLEAR_ALL:
      return {
        ...initialState,
        workplaces: state.workplaces
      }
      
    default:
      return state
  }
}

export const clearAll = function () {
  return {
    type: CLEAR_ALL
  }
}

export const setTask = function (data) {
  return {
    type: SET_TASK_FORM,
    data: data || {}
  }
}


export const setTasks = function (page, data) {
  return {
    type: SET_TASKS,
    page,
    data
  }
}

export const getWorkplaces = function () {
  return async function (dispatch) {
    try {
      const data = await fetchWorkplaces()
      dispatch({
        type: SET_WORK_PLACES,
        data
      })
    }
    catch (err) {
      dispatch({
        type: SET_WORK_PLACES,
        data: null
      })
    }
  }
}

export const getWorkplaceUsers = function (id, name) {
  return async function (dispatch) {
    dispatch({
      type: SET_USERS,
      data: null
    })
    
    try {
      const users = await fetchUsers(id)
      dispatch({
        type: SET_USERS,
        data: {
          list: users,
          name
        }
      })
    }

    catch (err) {
      dispatch({
        type: SET_USERS,
        data: null
      })
    }
  }
}

export const loadTasks = function (page) {
  return async function (dispatch, getState) {
    const userId = getState().user && getState().user.id
    if (!userId) {
      return
    }

    try {
      const data = await fetchTasks(userId, page)
      dispatch(setTasks(page, data))
    }

    catch (err) {
      dispatch(setTask(page, null))
    }
  }
}

export const loadWorkers = function () {
  return async function (dispatch) {
    try {
      const data = await fetchWorkers()
      dispatch({
        type: SET_WORKERS,
        data
      })
    }

    catch (err) {
      dispatch({
        type: SET_WORKERS,
        data: null
      })
    }
  }
}

export const updateTask = function (data, page) {
  return async function (dispatch, getState) {
    const tasks = getState().data.page || [data]
    dispatch(setTasks(page, tasks.map(x => x.id === data.id ? data : x)))
    dispatch({
      type: SET_TASK_DETAILS,
      data
    })
  }
}

export const setTaskDetails = function (data) {
  return {
    type: SET_TASK_DETAILS,
    data
  }
}

export const loadTaskDetails = function (taskId) {
  return async function (dispatch, getState) {
    const userId = getState().user && getState().user.id
    if (!userId) {
      return
    }
    
    dispatch(setTaskDetails(false))

    try {
      const data = await fetchTaskDetails(userId, taskId)
      taskId === data.id && dispatch(setTaskDetails(data))
    }

    catch (err) {
      dispatch(setTaskDetails(null))
    }
  }
}

export const addComment = function (comment) {
  return {
    type: ADD_COMMENT,
    comment
  }
}