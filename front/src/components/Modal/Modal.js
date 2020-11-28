import React from 'react'
import { useRouteMatch } from 'react-router'
import { createPathURL } from '../../shared/helpers'
import { Button } from '../../shared'
import './modal.scss'

const Modal = function (props) {
  const { title, buttons, children } = props 
  const match = useRouteMatch({ path: '*/:modal' })
  
  return <div className='modal'>
    <div className='modal-window'>
      {title && <div className='modal-title'>{ title }</div> }
      <div className='modal-content'>
        {children}
      </div>
      <div className='modal-buttons'>
        {buttons || null}
        <Button light to={createPathURL(match.path, { ...match.params, modal: undefined})}>
          Отмена
        </Button>
      </div>
    </div>
  </div>
}

export default Modal