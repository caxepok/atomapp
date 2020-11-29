import React from 'react'
import './page.scss'

export const PageMessage = function (props) {
  return <div className='page-message'>
    { props.children }
  </div>
}
