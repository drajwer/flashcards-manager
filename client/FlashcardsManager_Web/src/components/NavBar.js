import React from 'react';

import {
  Toolbar,
  ToolbarButton,
  BackButton,
  Icon
} from 'react-onsenui';

export default class NavBar extends React.Component {
  render() {
    const { title, backButton, onButtonClick } = this.props;
    return (
      <Toolbar>
        <div className='left'>
          {
            this.props.backButton
              ? <BackButton onClick={onButtonClick}>Powrót</BackButton>
              : (
                <ToolbarButton onClick={onButtonClick}>
                  <Icon icon='ion-navicon, material:md-menu' />
                </ToolbarButton>
              )
          }
        </div>
        <div className='center'>{title}</div>
      </Toolbar>
    );
  }

}

