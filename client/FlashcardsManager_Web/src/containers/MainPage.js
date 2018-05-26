import React from 'react';

import BasePageWithMenu from './base/BasePageWithMenu';
export default class MainPage extends BasePageWithMenu {
  resetState() {
  }
  renderPage() {
    return (
      <div style={{ margin: '30px 15px 25px 15px ' }} >
        <div className="card">
          <h2 className="card__title" style={{ fontWeight: 'bold' }}>Cześć!</h2>
          <div className="card__content">Wybierz z menu zakładkę "Nauka" i zacznij uczyć się z nami już teraz.</div>
        </div>
      </div>
    )
  }
}