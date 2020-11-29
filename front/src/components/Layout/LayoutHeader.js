import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import Menu from '../Menu'
import './layout.scss'
import { setUser } from '../../reducers/user'
import { Button } from '../../shared'
import { clearAll } from '../../reducers/data'

const LayoutHeader = function () {
  const userId = useSelector(({ user }) => user && user.id)
  const dispatch = useDispatch()
  
  const handleLogout = function () {
    dispatch(setUser())
    dispatch(clearAll())
  }

  return <div className='layout-header'>
    <div className='layout-title'>{
      userId ? 'Задачи' : 'Авторизация' 
    }</div>
    { userId && <Menu /> }
    { userId && <Button small light to={'/demo'}>
      Демо-видео
    </Button> }
    { userId && <Button small onClick={handleLogout}>
      Выйти
    </Button>}
  </div>  
}

export default LayoutHeader