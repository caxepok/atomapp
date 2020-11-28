import React from 'react'
import LayoutHeader from './LayoutHeader'
import LayoutSection from './LayoutSection'
import './layout.scss'

const Layout = function (props) {
  return <div className='layout'>
    <LayoutHeader/>
    {props.children}
  </div>
}

export default Layout